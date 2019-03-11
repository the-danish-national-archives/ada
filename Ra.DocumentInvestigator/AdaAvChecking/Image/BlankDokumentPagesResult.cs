namespace Ra.DocumentInvestigator.AdaAvChecking.Image
{
    public enum BlankDokumentPagesResult
    {
        OK = 0,

        UKENDT_FEJL = -1,

        INDEHOLDER_INGEN_SIDER = -2,

        ALLE_SIDER_TOMME = -3,

        OVERSKRIDER_MAX_ALLOWED_OG_ER_DERFOR_SUSPECT_OG_MULIGVIS_TOMT = -4
    }
}