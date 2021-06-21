BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "BinaryProperties" (
	"Id"	UNIQUEIDENTIFIER NOT NULL,
	"Length"	BIGINT NOT NULL CHECK("Length" >= 0),
	"Hash"	BINARY(16) DEFAULT NULL CHECK("Hash" IS NULL OR length("HASH") = 16),
	"UpstreamId"	UNIQUEIDENTIFIER DEFAULT NULL,
	"LastSynchronizedOn"	DATETIME DEFAULT NULL,
	"CreatedOn"	DATETIME NOT NULL DEFAULT (datetime('now', 'localtime')),
	"ModifiedOn"	DATETIME NOT NULL DEFAULT (datetime('now', 'localtime')),
	CONSTRAINT "PK_BinaryProperties" PRIMARY KEY("Id"),
	CONSTRAINT "UK_LengthHash" UNIQUE("Length","Hash"),
	CHECK("CreatedOn" <= "ModifiedOn" AND ("UpstreamId" IS NULL OR "LastSynchronizedOn" IS NOT NULL))
);
INSERT INTO "BinaryProperties" VALUES ('{6696e337-c4ad-4e03-b954-ee585270958d}',77824,'fb010000fb010000',NULL,NULL,'2021-05-21 21:49:59','2021-05-21 21:49:59');
INSERT INTO "BinaryProperties" VALUES ('{dc508120-8617-4d61-ba38-480ac35fcfe5}',0,X'fb010000fb010000fb010000fb010000',NULL,NULL,'2021-05-21 21:49:59','2021-05-21 21:49:59');
COMMIT;
