namespace FsInfoCat.Test.Helpers
{
    /// <summary>
    /// Encapsulates a method that has 1 output parameter and does not return a value.
    /// </summary>
    /// <typeparam name="R">The type of the output parameter.</typeparam>
    /// <param name="r">The output parameter.</param>
    public delegate void ActionWithOutput<R>(out R r);

    /// <summary>
    /// Encapsulates a method that has 1 parameter, 1 output parameter, and does not return a value.
    /// </summary>
    /// <typeparam name="T">The type of the first parameter.</typeparam>
    /// <typeparam name="R">The type of the output parameter.</typeparam>
    /// <param name="t">The first parameter.</param>
    /// <param name="r">The output parameter.</param>
    public delegate void ActionWithOutput<T, R>(T t, out R r);

    /// <summary>
    /// Encapsulates a method that has 2 parameters, 1 output parameter, and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="R">The type of the output parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="r">The output parameter.</param>
    public delegate void ActionWithOutput<T1, T2, R>(T1 t1, T2 t2, out R r);

    /// <summary>
    /// Encapsulates a method that has 3 parameters, 1 output parameter, and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="R">The type of the output parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="r">The output parameter.</param>
    public delegate void ActionWithOutput<T1, T2, T3, R>(T1 t1, T2 t2, T3 t3, out R r);

    /// <summary>
    /// Encapsulates a method that has 4 parameters, 1 output parameter, and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="R">The type of the output parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="t4">The fourth parameter.</param>
    /// <param name="r">The output parameter.</param>
    public delegate void ActionWithOutput<T1, T2, T3, T4, R>(T1 t1, T2 t2, T3 t3, T4 t4, out R r);

    /// <summary>
    /// Encapsulates a method that has 5 parameters, 1 output parameter, and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="R">The type of the output parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="t4">The fourth parameter.</param>
    /// <param name="t5">The fifth parameter.</param>
    /// <param name="r">The output parameter.</param>
    public delegate void ActionWithOutput<T1, T2, T3, T4, T5, R>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, out R r);

    /// <summary>
    /// Encapsulates a method that has 6 parameters, 1 output parameter, and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter.</typeparam>
    /// <typeparam name="R">The type of the output parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="t4">The fourth parameter.</param>
    /// <param name="t5">The fifth parameter.</param>
    /// <param name="t6">The sixth parameter.</param>
    /// <param name="r">The output parameter.</param>
    public delegate void ActionWithOutput<T1, T2, T3, T4, T5, T6, R>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, out R r);

    /// <summary>
    /// Encapsulates a method that has 2 output parameters and does not return a value.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    public delegate void ActionWithOutput2<R1, R2>(out R1 r1, out R2 r2);

    /// <summary>
    /// Encapsulates a method that has 1 parameter, 2 output parameters, and does not return a value.
    /// </summary>
    /// <typeparam name="T">The type of the first parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <param name="t">The first parameter.</param>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    public delegate void ActionWithOutput2<T, R1, R2>(T t, out R1 r1, out R2 r2);

    /// <summary>
    /// Encapsulates a method that has 2 parameters, 2 output parameters, and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    public delegate void ActionWithOutput2<T1, T2, R1, R2>(T1 t1, T2 t2, out R1 r1, out R2 r2);

    /// <summary>
    /// Encapsulates a method that has 3 parameters, 2 output parameters, and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    public delegate void ActionWithOutput2<T1, T2, T3, R1, R2>(T1 t1, T2 t2, T3 t3, out R1 r1, out R2 r2);

    /// <summary>
    /// Encapsulates a method that has 4 parameters, 2 output parameters, and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="t4">The fourth parameter.</param>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    public delegate void ActionWithOutput2<T1, T2, T3, T4, R1, R2>(T1 t1, T2 t2, T3 t3, T4 t4, out R1 r1, out R2 r2);

    /// <summary>
    /// Encapsulates a method that has 5 parameters, 2 output parameters, and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="t4">The fourth parameter.</param>
    /// <param name="t5">The fifth parameter.</param>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    public delegate void ActionWithOutput2<T1, T2, T3, T4, T5, R1, R2>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, out R1 r1, out R2 r2);

    /// <summary>
    /// Encapsulates a method that has 3 output parameters and does not return a value.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    public delegate void ActionWithOutput3<R1, R2, R3>(out R1 r1, out R2 r2, out R3 r3);

    /// <summary>
    /// Encapsulates a method that has 1 parameter, 3 output parameters, and does not return a value.
    /// </summary>
    /// <typeparam name="T">The type of the first parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <param name="t">The first parameter.</param>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    public delegate void ActionWithOutput3<T, R1, R2, R3>(T t, out R1 r1, out R2 r2, out R3 r3);

    /// <summary>
    /// Encapsulates a method that has 2 parameters, 3 output parameters, and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    public delegate void ActionWithOutput3<T1, T2, R1, R2, R3>(T1 t1, T2 t2, out R1 r1, out R2 r2, out R3 r3);

    /// <summary>
    /// Encapsulates a method that has 3 parameters, 3 output parameters, and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    public delegate void ActionWithOutput3<T1, T2, T3, R1, R2, R3>(T1 t1, T2 t2, T3 t3, out R1 r1, out R2 r2, out R3 r3);

    /// <summary>
    /// Encapsulates a method that has 4 parameters, 3 output parameters, and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="t4">The fourth parameter.</param>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    public delegate void ActionWithOutput3<T1, T2, T3, T4, R1, R2, R3>(T1 t1, T2 t2, T3 t3, T4 t4, out R1 r1, out R2 r2, out R3 r3);

    /// <summary>
    /// Encapsulates a method that has 4 output parameters and does not return a value.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    /// <param name="r4">The fourth output parameter.</param>
    public delegate void ActionWithOutput4<R1, R2, R3, R4>(out R1 r1, out R2 r2, out R3 r3, out R4 r4);

    /// <summary>
    /// Encapsulates a method that has 1 parameter, 4 output parameters, and does not return a value.
    /// </summary>
    /// <typeparam name="T">The type of the first parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <param name="t">The first parameter.</param>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    /// <param name="r4">The fourth output parameter.</param>
    public delegate void ActionWithOutput4<T, R1, R2, R3, R4>(T t, out R1 r1, out R2 r2, out R3 r3, out R4 r4);

    /// <summary>
    /// Encapsulates a method that has 2 parameters, 4 output parameters, and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    /// <param name="r4">The fourth output parameter.</param>
    public delegate void ActionWithOutput4<T1, T2, R1, R2, R3, R4>(T1 t1, T2 t2, out R1 r1, out R2 r2, out R3 r3, out R4 r4);

    /// <summary>
    /// Encapsulates a method that has 3 parameters, 4 output parameters, and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="t3">The third parameter.</param>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    /// <param name="r4">The fourth output parameter.</param>
    public delegate void ActionWithOutput4<T1, T2, T3, R1, R2, R3, R4>(T1 t1, T2 t2, T3 t3, out R1 r1, out R2 r2, out R3 r3, out R4 r4);

    /// <summary>
    /// Encapsulates a method that has 5 output parameters and does not return a value.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="R5">The type of the fifth output parameter.</typeparam>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    /// <param name="r4">The fourth output parameter.</param>
    /// <param name="r5">The fifth output parameter.</param>
    public delegate void ActionWithOutput5<R1, R2, R3, R4, R5>(out R1 r1, out R2 r2, out R3 r3, out R4 r4, out R5 r5);

    /// <summary>
    /// Encapsulates a method that has 1 parameter, 5 output parameters, and does not return a value.
    /// </summary>
    /// <typeparam name="T">The type of the first parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="R5">The type of the fifth output parameter.</typeparam>
    /// <param name="t">The first parameter.</param>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    /// <param name="r4">The fourth output parameter.</param>
    /// <param name="r5">The fifth output parameter.</param>
    public delegate void ActionWithOutput5<T, R1, R2, R3, R4, R5>(T t, out R1 r1, out R2 r2, out R3 r3, out R4 r4, out R5 r5);

    /// <summary>
    /// Encapsulates a method that has 2 parameters, 5 output parameters, and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="R5">The type of the fifth output parameter.</typeparam>
    /// <param name="t1">The first parameter.</param>
    /// <param name="t2">The second parameter.</param>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    /// <param name="r4">The fourth output parameter.</param>
    /// <param name="r5">The fifth output parameter.</param>
    public delegate void ActionWithOutput5<T1, T2, R1, R2, R3, R4, R5>(T1 t1, T2 t2, out R1 r1, out R2 r2, out R3 r3, out R4 r4, out R5 r5);

    /// <summary>
    /// Encapsulates a method that has 6 output parameters and does not return a value.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="R5">The type of the fifth output parameter.</typeparam>
    /// <typeparam name="R6">The type of the sixth output parameter.</typeparam>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    /// <param name="r4">The fourth output parameter.</param>
    /// <param name="r5">The fifth output parameter.</param>
    /// <param name="r6">The sixth output parameter.</param>
    public delegate void ActionWithOutput6<R1, R2, R3, R4, R5, R6>(out R1 r1, out R2 r2, out R3 r3, out R4 r4, out R5 r5, out R6 r6);

    /// <summary>
    /// Encapsulates a method that has 1 parameter, 6 output parameters, and does not return a value.
    /// </summary>
    /// <typeparam name="T">The type of the first parameter.</typeparam>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="R5">The type of the fifth output parameter.</typeparam>
    /// <typeparam name="R6">The type of the sixth output parameter.</typeparam>
    /// <param name="t">The first parameter.</param>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    /// <param name="r4">The fourth output parameter.</param>
    /// <param name="r5">The fifth output parameter.</param>
    /// <param name="r6">The sixth output parameter.</param>
    public delegate void ActionWithOutput6<T, R1, R2, R3, R4, R5, R6>(T t, out R1 r1, out R2 r2, out R3 r3, out R4 r4, out R5 r5, out R6 r6);

    /// <summary>
    /// Encapsulates a method that has 7 output parameters and does not return a value.
    /// </summary>
    /// <typeparam name="R1">The type of the first output parameter.</typeparam>
    /// <typeparam name="R2">The type of the second output parameter.</typeparam>
    /// <typeparam name="R3">The type of the third output parameter.</typeparam>
    /// <typeparam name="R4">The type of the fourth output parameter.</typeparam>
    /// <typeparam name="R5">The type of the fifth output parameter.</typeparam>
    /// <typeparam name="R6">The type of the sixth output parameter.</typeparam>
    /// <typeparam name="R7">The type of the seventh output parameter.</typeparam>
    /// <param name="r1">The first output parameter.</param>
    /// <param name="r2">The second output parameter.</param>
    /// <param name="r3">The third output parameter.</param>
    /// <param name="r4">The fourth output parameter.</param>
    /// <param name="r5">The fifth output parameter.</param>
    /// <param name="r6">The sixth output parameter.</param>
    /// <param name="r7">The seventh output parameter.</param>
    public delegate void ActionWithOutput7<R1, R2, R3, R4, R5, R6, R7>(out R1 r1, out R2 r2, out R3 r3, out R4 r4, out R5 r5, out R6 r6, out R7 r7);
}
