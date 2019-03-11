namespace Ra.DocumentInvestigator.AdaAvChecking.audioVideo
{
    /// <summary>
    ///     -6 Lyd ulovlig
    ///     -2 Ugyldig DLL version
    ///     -1 Ukendt fejl
    ///     0 ingen streams
    ///     1 Lyd streams er OK
    /// </summary>
    public enum AudioCompressionTestvalue
    {
        LydUlovlig = -6,
        UgyldigDLLVersion = -2,
        UkendtFejl = -1,
        IngenStreams = 0,
        LydStreamsErOK = 1
    }
}