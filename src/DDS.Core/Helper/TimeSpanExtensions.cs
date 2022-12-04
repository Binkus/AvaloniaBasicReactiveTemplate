using System.Runtime.CompilerServices;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable MemberCanBePrivate.Global

namespace DDS.Core.Helper;

[SuppressMessage("Roslynator", "RCS1213:Remove unused member declaration.")]
public static class TimeSpanExtensions
{
    /// <summary>Awaitable TimeSpan</summary>
    /// <inheritdoc cref="Task.Delay(TimeSpan)"/>
    /// <returns>An awaiter instance of task that represents the time delay.</returns>
    public static TaskAwaiter GetAwaiter(this TimeSpan delay) => Task.Delay(delay).GetAwaiter();
    
    /// <summary>Awaitable TimeSpan with option to continueOnCapturedContext or not.</summary>
    /// <param name="delay"><inheritdoc cref="Task.Delay(TimeSpan)"/></param>
    /// <param name="continueOnCapturedContext">
    /// <see langword="true" /> to attempt to marshal the continuation back to the original context captured; otherwise, <see langword="false" />.</param>
    /// <returns>An object used to await this task that represents the time delay.</returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException"><inheritdoc cref="Task.Delay(TimeSpan)"/></exception>
    public static ConfiguredTaskAwaitable ConfigureAwait(this TimeSpan delay, bool continueOnCapturedContext)
    {
        return Task.Delay(delay).ConfigureAwait(continueOnCapturedContext);
    }
    
    /// <inheritdoc cref="Task.Delay(TimeSpan, CancellationToken)"/>
    public static Task Delay(this TimeSpan delay, CancellationToken cancellationToken = default)
        => Task.Delay(delay, cancellationToken);
    
    /// <summary>
    /// <p>Not continuing on captured context:
    /// No attempt to marshal the continuation back to the originally captured context.</p>
    /// <p>Equivalent to "Task.Delay(TimeSpan, CancellationToken).ConfigureAwait(false)"</p>
    /// <inheritdoc cref="Task.Delay(TimeSpan, CancellationToken)"/>
    /// </summary>
    /// <inheritdoc cref="Task.Delay(TimeSpan, CancellationToken)"/>
    /// <returns>An object used to await this task that represents the time delay.</returns>
    public static ConfiguredTaskAwaitable DelayWithoutContinuingOnCapturedContext(
        this TimeSpan delay, CancellationToken cancellationToken = default)
        => Task.Delay(delay, cancellationToken).ConfigureAwait(false);
    
    /// <summary>
    /// Creates a duration as TimeSpan which can be awaited by Task.Delay or directly through ext method
    /// <see cref="GetAwaiter"/>.
    /// <p>E.g. await 5.Seconds() or 5.s() which is equal to await Task.Delay(TimeSpan.FromSeconds(5)).</p>
    /// <p>Alternatively you can use ConfigureAwait(false) if you do not wish to continueOnCapturedContext,
    /// e.g. by await 5.Seconds().ConfigureAwait(false)</p>
    /// <p><b>Examples:</b></p>
    /// <p>await 42.s().ConfigureAwait(false);</p>
    /// <p>await 42.s().Delay(cancellationToken).ConfigureAwait(false);</p>
    /// <p>await 42.s().DelayWithoutContinuingOnCapturedContext(cancellationToken);</p>
    /// </summary>
    /// <returns>TimeSpan / Duration / Delay of specified unit</returns>
    /// <seealso cref="Task.Delay(TimeSpan)"/>
    /// <seealso cref="Delay(TimeSpan, CancellationToken)"/>
    /// <seealso cref="DelayWithoutContinuingOnCapturedContext(TimeSpan, CancellationToken)"/>
    /// <seealso cref="GetAwaiter"/>
    /// <seealso cref="ConfigureAwait"/>
    /// <seealso cref="Seconds(int)"/>
    /// <seealso cref="Seconds(double)"/>
    /// <seealso cref="s(int)"/>
    /// <seealso cref="s(double)"/>
    /// <seealso cref="Milliseconds(int)"/>
    /// <seealso cref="Milliseconds(double)"/>
    /// <seealso cref="ms(int)"/>
    /// <seealso cref="ms(double)"/>
    /// <seealso cref="Microseconds(int)"/>
    /// <seealso cref="Microseconds(double)"/>
    /// <seealso cref="Hours(int)"/>
    /// <seealso cref="Hours(double)"/>
    /// <seealso cref="Minutes(int)"/>
    /// <seealso cref="Minutes(double)"/>
    /// <seealso cref="Ticks(int)"/>
    /// <seealso cref="Ticks(long)"/>
    private static TimeSpan NumbersExtToTimeSpanDocs() => TimeSpan.Zero;
    
    /// <inheritdoc cref="NumbersExtToTimeSpanDocs"/>
    public static TimeSpan Seconds(this int seconds) => TimeSpan.FromSeconds(seconds);
    /// <inheritdoc cref="NumbersExtToTimeSpanDocs"/>
    public static TimeSpan Seconds(this double seconds) => TimeSpan.FromSeconds(seconds);
    
    /// <inheritdoc cref="NumbersExtToTimeSpanDocs"/>
    public static TimeSpan Milliseconds(this int milliseconds) => TimeSpan.FromMilliseconds(milliseconds);
    /// <inheritdoc cref="NumbersExtToTimeSpanDocs"/>
    public static TimeSpan Milliseconds(this double milliseconds) => TimeSpan.FromMilliseconds(milliseconds);
    
    /// <inheritdoc cref="NumbersExtToTimeSpanDocs"/>
    public static TimeSpan Microseconds(this int microseconds) => TimeSpan.FromMicroseconds(microseconds);
    /// <inheritdoc cref="NumbersExtToTimeSpanDocs"/>
    public static TimeSpan Microseconds(this double microseconds) => TimeSpan.FromMicroseconds(microseconds);
    
    /// <inheritdoc cref="NumbersExtToTimeSpanDocs"/>
    public static TimeSpan Hours(this int hours) => TimeSpan.FromHours(hours);
    /// <inheritdoc cref="NumbersExtToTimeSpanDocs"/>
    public static TimeSpan Hours(this double hours) => TimeSpan.FromHours(hours);
    
    /// <inheritdoc cref="NumbersExtToTimeSpanDocs"/>
    public static TimeSpan Minutes(this int minutes) => TimeSpan.FromMinutes(minutes);
    /// <inheritdoc cref="NumbersExtToTimeSpanDocs"/>
    public static TimeSpan Minutes(this double minutes) => TimeSpan.FromMinutes(minutes);

    /// <inheritdoc cref="NumbersExtToTimeSpanDocs"/>
    public static TimeSpan Ticks(this int ticks) => TimeSpan.FromTicks(ticks);
    /// <inheritdoc cref="NumbersExtToTimeSpanDocs"/>
    public static TimeSpan Ticks(this long ticks) => TimeSpan.FromTicks(ticks);
    
    // Abbreviations
    
    /// <inheritdoc cref="Milliseconds(int)"/>
    public static TimeSpan ms(this int milliseconds) => Milliseconds(milliseconds);
    /// <inheritdoc cref="Milliseconds(double)"/>
    public static TimeSpan ms(this double milliseconds) => Milliseconds(milliseconds);
    
    /// <inheritdoc cref="Seconds(int)"/>
    public static TimeSpan s(this int seconds) => Seconds(seconds);
    /// <inheritdoc cref="Seconds(double)"/>
    public static TimeSpan s(this double seconds) => Seconds(seconds);
}