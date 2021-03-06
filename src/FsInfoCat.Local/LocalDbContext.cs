using FsInfoCat.Collections;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public partial class LocalDbContext : BaseDbContext, ILocalDbContext
    {
        private static readonly object _syncRoot = new();
        private static bool _connectionStringValidated;
        private readonly ILogger<LocalDbContext> _logger;

        public virtual DbSet<FileSystem> FileSystems { get; set; }

        public virtual DbSet<SymbolicName> SymbolicNames { get; set; }

        public virtual DbSet<Volume> Volumes { get; set; }

        public virtual DbSet<VolumeAccessError> VolumeAccessErrors { get; set; }

        public virtual DbSet<Subdirectory> Subdirectories { get; set; }

        public virtual DbSet<SubdirectoryAccessError> SubdirectoryAccessErrors { get; set; }

        public virtual DbSet<DbFile> Files { get; set; }

        public virtual DbSet<FileAccessError> FileAccessErrors { get; set; }

        public virtual DbSet<SummaryPropertySet> SummaryPropertySets { get; }

        public virtual DbSet<DocumentPropertySet> DocumentPropertySets { get; }

        public virtual DbSet<AudioPropertySet> AudioPropertySets { get; }

        public virtual DbSet<DRMPropertySet> DRMPropertySets { get; }

        public virtual DbSet<GPSPropertySet> GPSPropertySets { get; }

        public virtual DbSet<ImagePropertySet> ImagePropertySets { get; }

        public virtual DbSet<MediaPropertySet> MediaPropertySets { get; }

        public virtual DbSet<MusicPropertySet> MusicPropertySets { get; }

        public virtual DbSet<PhotoPropertySet> PhotoPropertySets { get; }

        public virtual DbSet<RecordedTVPropertySet> RecordedTVPropertySets { get; }

        public virtual DbSet<VideoPropertySet> VideoPropertySets { get; }

        public virtual DbSet<BinaryPropertySet> BinaryPropertySets { get; set; }

        public virtual DbSet<FileComparison> Comparisons { get; set; }

        public virtual DbSet<RedundantSet> RedundantSets { get; set; }

        public virtual DbSet<Redundancy> Redundancies { get; set; }

        public virtual DbSet<CrawlConfiguration> CrawlConfigurations { get; set; }

        public LocalDbContext(DbContextOptions<LocalDbContext> options)
            : base(options)
        //: base(Services.ServiceProvider.GetRequiredService<ILogger<LocalDbContext>>(), options)
        {
            _logger = Services.ServiceProvider.GetRequiredService<ILogger<LocalDbContext>>();
            lock (_syncRoot)
            {
                if (!_connectionStringValidated)
                {
                    _connectionStringValidated = true;
                    SqliteConnectionStringBuilder builder = new(Database.GetConnectionString());
                    string connectionString = builder.ConnectionString;
                    _logger.LogInformation($"Using {nameof(SqliteConnectionStringBuilder.ConnectionString)} {{{nameof(SqliteConnectionStringBuilder.ConnectionString)}}}",
                        connectionString);
                    if (!File.Exists(builder.DataSource))
                    {
                        builder.Mode = SqliteOpenMode.ReadWriteCreate;
                        _logger.LogInformation("Initializing new database");
                        using SqliteConnection connection = new(builder.ConnectionString);
                        connection.Open();
                        foreach (var element in XDocument.Parse(Properties.Resources.DbCommands).Root.Elements("DbCreation").Elements("Text"))
                        {
                            _logger.LogTrace($"{{Message}}; {nameof(SqliteCommand)}={{{nameof(SqliteCommand.CommandText)}}}",
                                element.Attributes("Message").Select(a => a.Value).DefaultIfEmpty("").First(), element.Value);
                            using SqliteCommand command = connection.CreateCommand();
                            command.CommandText = element.Value;
                            command.CommandType = System.Data.CommandType.Text;
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Inherited class will have called SuppressFinalize if necessary.")]
        public override void Dispose()
        {
            _logger.LogInformation($"Disposing {nameof(LocalDbContext)}: {nameof(DbContextId.InstanceId)}={{{nameof(DbContextId.InstanceId)}}}, {nameof(DbContextId.Lease)}={{{nameof(DbContextId.Lease)}}}",
                ContextId.InstanceId, ContextId.Lease);
            base.Dispose();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<SymbolicName>(SymbolicName.BuildEntity);
            modelBuilder.Entity<Volume>(Volume.BuildEntity);
            modelBuilder.Entity<Subdirectory>(Subdirectory.BuildEntity);
            modelBuilder.Entity<DbFile>(DbFile.BuildEntity);
            modelBuilder.Entity<BinaryPropertySet>(BinaryPropertySet.BuildEntity);
            modelBuilder.Entity<FileComparison>(FileComparison.BuildEntity);
            modelBuilder.Entity<RedundantSet>(RedundantSet.BuildEntity);
            modelBuilder.Entity<Redundancy>(Redundancy.BuildEntity);
            modelBuilder.Entity<SummaryPropertySet>(SummaryPropertySet.BuildEntity);
            modelBuilder.Entity<DocumentPropertySet>(DocumentPropertySet.BuildEntity);
            modelBuilder.Entity<GPSPropertySet>(GPSPropertySet.BuildEntity);
            modelBuilder.Entity<MediaPropertySet>(MediaPropertySet.BuildEntity);
            modelBuilder.Entity<MusicPropertySet>(MusicPropertySet.BuildEntity);
            modelBuilder.Entity<PhotoPropertySet>(PhotoPropertySet.BuildEntity);
            modelBuilder.Entity<VideoPropertySet>(VideoPropertySet.BuildEntity);
        }

        public static void AddDbContextPool(IServiceCollection services, Assembly assembly, string dbFileName) =>
            AddDbContextPool(services, GetDbFilePath(assembly, dbFileName));

        public static void AddDbContextPool(IServiceCollection services, string dbPath)
        {
            string connectionString = GetConnectionString(dbPath);
            services.AddDbContextPool<LocalDbContext>(options => options.AddInterceptors(new Interceptor()).UseSqlite(connectionString));
            // this.Model.GetDefaultSchema();
        }

        public static string GetConnectionString(Assembly assembly, string dbFileName) => GetConnectionString(GetDbFilePath(assembly, dbFileName));

        public static string GetConnectionString(string dbPath)
        {
            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = dbPath,
                ForeignKeys = true,
                Mode = SqliteOpenMode.ReadWrite
            };
            return builder.ConnectionString;
        }

        public static string GetDbFilePath(Assembly assembly, string dbFileName)
        {
            if (string.IsNullOrWhiteSpace(dbFileName))
                dbFileName = Services.DEFAULT_LOCAL_DB_FILENAME;
            if (Path.IsPathFullyQualified(dbFileName))
                return Path.GetFullPath(dbFileName);
            return Path.Combine(Services.GetAppDataPath(assembly), dbFileName);
        }

        [Obsolete("Pass cancellation token")]
        public async Task<IEnumerable<TProperty>> GetRelatedCollectionAsync<TEntity, TProperty>([DisallowNull] TEntity entity,
            [DisallowNull] Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression)
            where TEntity : class
            where TProperty : class => await Entry(entity).GetRelatedCollectionAsync(propertyExpression);

        public async Task<IEnumerable<TProperty>> GetRelatedCollectionAsync<TEntity, TProperty>([DisallowNull] TEntity entity,
            [DisallowNull] Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression, CancellationToken cancellationToken)
            where TEntity : class
            where TProperty : class => await Entry(entity).GetRelatedCollectionAsync(propertyExpression, cancellationToken);

        public async Task<SummaryPropertySet> FindMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string applicationName = properties.ApplicationName.TrimmedOrNullIfWhiteSpace();
            MultiStringValue author = MultiStringValue.NullIfNotAny(properties.Author);
            // TODO: Implement FindMatchingAsync(ISummaryProperties, CancellationToken);
            return await SummaryPropertySets.FirstOrDefaultAsync(p => p.ApplicationName == applicationName);
        }

        public async Task<DocumentPropertySet> FindMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string clientID = properties.ClientID.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IDocumentProperties, CancellationToken);
            return await DocumentPropertySets.FirstOrDefaultAsync(p => p.ClientID == clientID);
        }

        public async Task<AudioPropertySet> FindMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string format = properties.Format.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IAudioProperties, CancellationToken);
            return await AudioPropertySets.FirstOrDefaultAsync(p => p.Format == format);
        }

        public async Task<DRMPropertySet> FindMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string description = properties.Description.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IDRMProperties, CancellationToken);
            return await DRMPropertySets.FirstOrDefaultAsync(p => p.Description == description);
        }

        public async Task<GPSPropertySet> FindMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string areaInformation = properties.AreaInformation.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IGPSProperties, CancellationToken);
            return await GPSPropertySets.FirstOrDefaultAsync(p => p.AreaInformation == areaInformation);
        }

        public async Task<ImagePropertySet> FindMatchingAsync(IImageProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string imageID = properties.ImageID.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IImageProperties, CancellationToken);
            return await ImagePropertySets.FirstOrDefaultAsync(p => p.ImageID == imageID);
        }

        public async Task<MediaPropertySet> FindMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string dvdID = properties.DVDID.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IMediaProperties, CancellationToken);
            return await MediaPropertySets.FirstOrDefaultAsync(p => p.DVDID == dvdID);
        }

        public async Task<MusicPropertySet> FindMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string displayArtist = properties.DisplayArtist.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IMusicProperties, CancellationToken);
            return await MusicPropertySets.FirstOrDefaultAsync(p => p.DisplayArtist == displayArtist);
        }

        public async Task<PhotoPropertySet> FindMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string exifVersion = properties.EXIFVersion.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IPhotoProperties, CancellationToken);
            return await PhotoPropertySets.FirstOrDefaultAsync(p => p.EXIFVersion == exifVersion);
        }

        public async Task<RecordedTVPropertySet> FindMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string episodeName = properties.EpisodeName.TrimmedOrNullIfWhiteSpace();
            // TODO: Implement FindMatchingAsync(IRecordedTVProperties, CancellationToken);
            return await RecordedTVPropertySets.FirstOrDefaultAsync(p => p.EpisodeName == episodeName);
        }

        public async Task<VideoPropertySet> FindMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            string compression = properties.Compression.NullIfWhiteSpaceOrNormalized();
            // TODO: Implement FindMatchingAsync(IVideoProperties, CancellationToken);
            return await VideoPropertySets.FirstOrDefaultAsync(p => p.Compression == compression);
        }

        public async Task<SummaryPropertySet> GetMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            SummaryPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(ISummaryProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<DocumentPropertySet> GetMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            DocumentPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IDocumentProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<AudioPropertySet> GetMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            AudioPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IAudioProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<DRMPropertySet> GetMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            DRMPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IDRMProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<GPSPropertySet> GetMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            GPSPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IGPSProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<ImagePropertySet> GetMatchingAsync(IImageProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            ImagePropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IImageProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<MediaPropertySet> GetMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            MediaPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IMediaProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<MusicPropertySet> GetMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            MusicPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IMusicProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<PhotoPropertySet> GetMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            PhotoPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IPhotoProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<RecordedTVPropertySet> GetMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            RecordedTVPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IRecordedTVProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        public async Task<VideoPropertySet> GetMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken)
        {
            if (properties.IsNullOrAllPropertiesEmpty())
                return null;
            VideoPropertySet result = await FindMatchingAsync(properties, cancellationToken);
            if (result is null)
            {
                // TODO: Implement GetMatchingAsync(IVideoProperties, CancellationToken);
                throw new NotImplementedException();
            }
            return result;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(BinaryPropertySet binaryProperties)
        {
            EntityEntry<BinaryPropertySet> entry = Entry(binaryProperties);
            if ((await entry.GetRelatedCollectionAsync(p => p.Files)).Any() || !(await entry.GetRelatedCollectionAsync(p => p.RedundantSets)).Any())
                return false;
            BinaryPropertySets.Remove(binaryProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(BinaryPropertySet binaryProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            EntityEntry<BinaryPropertySet> entry = Entry(binaryProperties);
            if ((await entry.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any() || !(await entry.GetRelatedCollectionAsync(p => p.RedundantSets, cancellationToken)).Any())
                return false;
            BinaryPropertySets.Remove(binaryProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(VideoPropertySet videoProperties)
        {
            if ((await GetRelatedCollectionAsync(videoProperties, p => p.Files)).Any())
                return false;
            VideoPropertySets.Remove(videoProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(VideoPropertySet videoProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(videoProperties, p => p.Files, cancellationToken)).Any())
                return false;
            VideoPropertySets.Remove(videoProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(RecordedTVPropertySet recordedTVProperties)
        {
            if ((await GetRelatedCollectionAsync(recordedTVProperties, p => p.Files)).Any())
                return false;
            RecordedTVPropertySets.Remove(recordedTVProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(RecordedTVPropertySet recordedTVProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(recordedTVProperties, p => p.Files, cancellationToken)).Any())
                return false;
            RecordedTVPropertySets.Remove(recordedTVProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(PhotoPropertySet photoProperties)
        {
            if ((await GetRelatedCollectionAsync(photoProperties, p => p.Files)).Any())
                return false;
            PhotoPropertySets.Remove(photoProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(PhotoPropertySet photoProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(photoProperties, p => p.Files, cancellationToken)).Any())
                return false;
            PhotoPropertySets.Remove(photoProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(MusicPropertySet musicProperties)
        {
            if ((await GetRelatedCollectionAsync(musicProperties, p => p.Files)).Any())
                return false;
            MusicPropertySets.Remove(musicProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(MusicPropertySet musicProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(musicProperties, p => p.Files, cancellationToken)).Any())
                return false;
            MusicPropertySets.Remove(musicProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(MediaPropertySet mediaProperties)
        {
            if ((await GetRelatedCollectionAsync(mediaProperties, p => p.Files)).Any())
                return false;
            MediaPropertySets.Remove(mediaProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(MediaPropertySet mediaProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(mediaProperties, p => p.Files, cancellationToken)).Any())
                return false;
            MediaPropertySets.Remove(mediaProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(ImagePropertySet imageProperties)
        {
            if ((await GetRelatedCollectionAsync(imageProperties, p => p.Files)).Any())
                return false;
            ImagePropertySets.Remove(imageProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(ImagePropertySet imageProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(imageProperties, p => p.Files, cancellationToken)).Any())
                return false;
            ImagePropertySets.Remove(imageProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(GPSPropertySet gpsProperties)
        {
            if ((await GetRelatedCollectionAsync(gpsProperties, p => p.Files)).Any())
                return false;
            GPSPropertySets.Remove(gpsProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(GPSPropertySet gpsProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(gpsProperties, p => p.Files, cancellationToken)).Any())
                return false;
            GPSPropertySets.Remove(gpsProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(DRMPropertySet drmProperties)
        {
            if ((await GetRelatedCollectionAsync(drmProperties, p => p.Files)).Any())
                return false;
            DRMPropertySets.Remove(drmProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(DRMPropertySet drmProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(drmProperties, p => p.Files, cancellationToken)).Any())
                return false;
            DRMPropertySets.Remove(drmProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(AudioPropertySet audioProperties)
        {
            if ((await GetRelatedCollectionAsync(audioProperties, p => p.Files)).Any())
                return false;
            AudioPropertySets.Remove(audioProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(AudioPropertySet audioProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(audioProperties, p => p.Files, cancellationToken)).Any())
                return false;
            AudioPropertySets.Remove(audioProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(DocumentPropertySet documentProperties)
        {
            if ((await GetRelatedCollectionAsync(documentProperties, p => p.Files)).Any())
                return false;
            DocumentPropertySets.Remove(documentProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(DocumentPropertySet documentProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(documentProperties, p => p.Files, cancellationToken)).Any())
                return false;
            DocumentPropertySets.Remove(documentProperties);
            return true;
        }

        [Obsolete("Pass cancellation token")]
        private async Task<bool> RemoveIfNoReferencesAsync(SummaryPropertySet summaryProperties)
        {
            if ((await GetRelatedCollectionAsync(summaryProperties, p => p.Files)).Any())
                return false;
            SummaryPropertySets.Remove(summaryProperties);
            return true;
        }

        private async Task<bool> RemoveIfNoReferencesAsync(SummaryPropertySet summaryProperties, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if ((await GetRelatedCollectionAsync(summaryProperties, p => p.Files, cancellationToken)).Any())
                return false;
            SummaryPropertySets.Remove(summaryProperties);
            return true;
        }

        private void ForceDeleteRedundancy(Redundancy target)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            var redundantSet = target.RedundantSet;
            Redundancies.Remove(target);
            SaveChanges();
            if (redundantSet.Redundancies.Count == 0)
                RedundantSets.Remove(redundantSet);
        }

        #region Overrides

        protected override IEnumerable<IComparison> GetGenericComparisons() => Comparisons;

        protected override IEnumerable<IBinaryPropertySet> GetGenericBinaryPropertySets() => BinaryPropertySets;

        protected override IEnumerable<ISummaryPropertySet> GetGenericSummaryPropertySets() => SummaryPropertySets;

        protected override IEnumerable<IDocumentPropertySet> GetGenericDocumentPropertySets() => DocumentPropertySets;

        protected override IEnumerable<IAudioPropertySet> GetGenericAudioPropertySets() => AudioPropertySets;

        protected override IEnumerable<IDRMPropertySet> GetGenericDRMPropertySets() => DRMPropertySets;

        protected override IEnumerable<IGPSPropertySet> GetGenericGPSPropertySets() => GPSPropertySets;

        protected override IEnumerable<IImagePropertySet> GetGenericImagePropertySets() => ImagePropertySets;

        protected override IEnumerable<IMediaPropertySet> GetGenericMediaPropertySets() => MediaPropertySets;

        protected override IEnumerable<IMusicPropertySet> GetGenericMusicPropertySets() => MusicPropertySets;

        protected override IEnumerable<IPhotoPropertySet> GetGenericPhotoPropertySets() => PhotoPropertySets;

        protected override IEnumerable<IRecordedTVPropertySet> GetGenericRecordedTVPropertySets() => RecordedTVPropertySets;

        protected override IEnumerable<IVideoPropertySet> GetGenericVideoPropertySets() => VideoPropertySets;

        protected override IEnumerable<IAccessError<IFile>> GetGenericFileAccessErrors() => FileAccessErrors;

        protected override IEnumerable<IFile> GetGenericFiles() => Files;

        protected override IEnumerable<IFileSystem> GetGenericFileSystems() => FileSystems;

        protected override IEnumerable<IRedundancy> GetGenericRedundancies() => Redundancies;

        protected override IEnumerable<IRedundantSet> GetGenericRedundantSets() => RedundantSets;

        protected override IEnumerable<ISubdirectory> GetGenericSubdirectories() => Subdirectories;

        protected override IEnumerable<IAccessError<ISubdirectory>> GetGenericSubdirectoryAccessErrors() => SubdirectoryAccessErrors;

        protected override IEnumerable<ISymbolicName> GetGenericSymbolicNames() => SymbolicNames;

        protected override IEnumerable<IAccessError<IVolume>> GetGenericVolumeAccessErrors() => VolumeAccessErrors;

        protected override IEnumerable<IVolume> GetGenericVolumes() => Volumes;

        protected override IEnumerable<ICrawlConfiguration> GetGenericCrawlConfigurations() => CrawlConfigurations;

        protected async override Task<IGPSPropertySet> FindGenericMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<IImagePropertySet> FindGenericMatchingAsync(IImageProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<IMediaPropertySet> FindGenericMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<IMusicPropertySet> FindGenericMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<IPhotoPropertySet> FindGenericMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<IRecordedTVPropertySet> FindGenericMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<IVideoPropertySet> FindGenericMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<ISummaryPropertySet> FindGenericMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<IDocumentPropertySet> FindGenericMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<IAudioPropertySet> FindGenericMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        protected async override Task<IDRMPropertySet> FindGenericMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        #endregion

        #region Explicit Members

        IEnumerable<ILocalComparison> ILocalDbContext.Comparisons => Comparisons;

        IEnumerable<ILocalBinaryPropertySet> ILocalDbContext.BinaryPropertySets => BinaryPropertySets;

        IEnumerable<ILocalSummaryPropertySet> ILocalDbContext.SummaryPropertySets => SummaryPropertySets;

        IEnumerable<ILocalDocumentPropertySet> ILocalDbContext.DocumentPropertySets => DocumentPropertySets;

        IEnumerable<ILocalAudioPropertySet> ILocalDbContext.AudioPropertySets => AudioPropertySets;

        IEnumerable<ILocalDRMPropertySet> ILocalDbContext.DRMPropertySets => DRMPropertySets;

        IEnumerable<ILocalGPSPropertySet> ILocalDbContext.GPSPropertySets => GPSPropertySets;

        IEnumerable<ILocalImagePropertySet> ILocalDbContext.ImagePropertySets => ImagePropertySets;

        IEnumerable<ILocalMediaPropertySet> ILocalDbContext.MediaPropertySets => MediaPropertySets;

        IEnumerable<ILocalMusicPropertySet> ILocalDbContext.MusicPropertySets => MusicPropertySets;

        IEnumerable<ILocalPhotoPropertySet> ILocalDbContext.PhotoPropertySets => PhotoPropertySets;

        IEnumerable<ILocalRecordedTVPropertySet> ILocalDbContext.RecordedTVPropertySets => RecordedTVPropertySets;

        IEnumerable<ILocalVideoPropertySet> ILocalDbContext.VideoPropertySets => VideoPropertySets;

        IEnumerable<IAccessError<ILocalFile>> ILocalDbContext.FileAccessErrors => FileAccessErrors;

        IEnumerable<ILocalFile> ILocalDbContext.Files => Files;

        IEnumerable<ILocalFileSystem> ILocalDbContext.FileSystems => FileSystems;

        IEnumerable<ILocalRedundancy> ILocalDbContext.Redundancies => Redundancies;

        IEnumerable<ILocalRedundantSet> ILocalDbContext.RedundantSets => RedundantSets;

        IEnumerable<ILocalSubdirectory> ILocalDbContext.Subdirectories => Subdirectories;

        IEnumerable<IAccessError<ILocalSubdirectory>> ILocalDbContext.SubdirectoryAccessErrors => SubdirectoryAccessErrors;

        IEnumerable<ILocalSymbolicName> ILocalDbContext.SymbolicNames => SymbolicNames;

        IEnumerable<IAccessError<ILocalVolume>> ILocalDbContext.VolumeAccessErrors => VolumeAccessErrors;

        IEnumerable<ILocalVolume> ILocalDbContext.Volumes => Volumes;

        IEnumerable<ILocalCrawlConfiguration> ILocalDbContext.CrawlConfigurations => CrawlConfigurations;

        async Task<ILocalSummaryPropertySet> ILocalDbContext.FindMatchingAsync(ISummaryProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalDocumentPropertySet> ILocalDbContext.FindMatchingAsync(IDocumentProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalAudioPropertySet> ILocalDbContext.FindMatchingAsync(IAudioProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalDRMPropertySet> ILocalDbContext.FindMatchingAsync(IDRMProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalGPSPropertySet> ILocalDbContext.FindMatchingAsync(IGPSProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalImagePropertySet> ILocalDbContext.FindMatchingAsync(IImageProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalMediaPropertySet> ILocalDbContext.FindMatchingAsync(IMediaProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalMusicPropertySet> ILocalDbContext.FindMatchingAsync(IMusicProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalPhotoPropertySet> ILocalDbContext.FindMatchingAsync(IPhotoProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalRecordedTVPropertySet> ILocalDbContext.FindMatchingAsync(IRecordedTVProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        async Task<ILocalVideoPropertySet> ILocalDbContext.FindMatchingAsync(IVideoProperties properties, CancellationToken cancellationToken) =>
            await FindMatchingAsync(properties, cancellationToken);

        #endregion
    }
}
