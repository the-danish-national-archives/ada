namespace Ra.DocumentInvestigator.AdaAvChecking.audioVideo
{
    /// <summary>
    ///     -7 Både Lyd og Video ulovlig
    ///     -6 Lyd ulovlig
    ///     -5 Video ulovlig
    ///     -2 Ugyldig DLL version
    ///     -1 Ukendt fejl
    ///     0 ingen streams
    ///     1 Lyd & Video streams er begge OK
    /// </summary>
    public enum VideoCompressionTestvalue
    {
        AudioAndVideoIllegal = -7,
        VideoIllegal = -5,
        AudioAndVideoOK = 1,
        AudioIllegal = -6,
        InvalidDLLVersion = -2,
        UnknownError = -1,
        NoStreams = 0
    }
}