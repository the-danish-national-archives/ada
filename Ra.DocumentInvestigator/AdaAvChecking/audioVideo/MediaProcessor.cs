namespace Ra.DocumentInvestigator.AdaAvChecking.audioVideo
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using log4net;

    #endregion

    public class MediaProcessor
//        : IMediaProcessor
    {
        #region  Fields

        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region

        //!!!!   benyttes ikke ....hvorfor ???   !!!!!!
        public List<string> GetMP3DocumentInfo(string filnavn)
        {
            var resultTagList = new List<string>();

            //Test if version of DLL is compatible : 3rd argument is "version of DLL tested;Your application name;Your application version"
            var mediaInfo = new MediaInfo();

            if (mediaInfo.Option("Info_Version").Length == 0)
            {
                _log.Error("MediaInfo fejl! - MediaInfo.Dll is not compatible");
                return resultTagList;
            }

            //!!! Mangler at returnere de korrekte tag data som der kan testes på !!!!

            mediaInfo.Open(filnavn);

            var streamCount = mediaInfo.Count_Get(StreamKind.General);

            for (var i = 0; i < streamCount; i++)
            {
                resultTagList.Add("Streams nr  " + i);

                if (mediaInfo.Count_Get(StreamKind.Video, i) > 0)
                {
                    resultTagList.Add("Video");
                    resultTagList.Add("Format:    " + mediaInfo.Get(StreamKind.Video, i, "Format"));
                    resultTagList.Add("Codec:     " + mediaInfo.Get(StreamKind.Video, i, "Codec/String"));
                    resultTagList.Add("Profile:   " + mediaInfo.Get(StreamKind.Video, i, "Format profile"));
                    resultTagList.Add("\n");
                }

                if (mediaInfo.Count_Get(StreamKind.Audio, i) > 0)
                {
                    resultTagList.Add("Audio");
                    resultTagList.Add("Format:    " + mediaInfo.Get(StreamKind.Audio, i, "Format"));
                    resultTagList.Add("Codec:     " + mediaInfo.Get(StreamKind.Audio, i, "Codec/String"));
                    resultTagList.Add("Profile:   " + mediaInfo.Get(StreamKind.Audio, i, "Format profile"));
                    resultTagList.Add("\n");
                }

                resultTagList.Add("\n");

                //resultTagList.Add("Antal streams  " + streamCount.ToString());
                ////if (MI.Count_Get(StreamKind.Audio) > 0)
                //{
                ////resultTagList.Add("Filnavn:       " + filnavn);
                ////resultTagList.Add("Antal streams: " + MI.Count_Get(StreamKind.General));
                //resultTagList.Add("Antal streams  " + MI.Get(StreamKind.General, 0, "GeneralCount"));
                //resultTagList.Add("Format:        " + MI.Get(StreamKind.General, 0, "Format"));
                //resultTagList.Add("ID:            " + MI.Get(StreamKind.Video, 0, "StreamKind/String"));
                ////resultTagList.Add("Filstørrelse:  " + MI.Option("Inform", "General;File size is %FileSize% bytes"));

                //resultTagList.Add("Bitrate:      " + 
                //resultTagList.Add("Frekvens:     " +
                //resultTagList.Add("Længde i sec: " + 
                //resultTagList.Add("Længde h:m:s: " + 
                //resultTagList.Add("Kanaler:      " +
            }

            return resultTagList;
        }

        private static bool TestAudio(MediaInfo mediaInfo, int i, bool hasAudio, ref bool audioResult)
        {
            if (mediaInfo.Count_Get(StreamKind.Audio, i) > 0)
            {
                hasAudio = true;
                var format = mediaInfo.Get(StreamKind.Audio, i, "Format");
                var Codec = mediaInfo.Get(StreamKind.Audio, i, "Codec/String");
                var Version = mediaInfo.Get(StreamKind.Audio, i, "Format_Version");
                var Profile = mediaInfo.Get(StreamKind.Audio, i, "Format profile");

                switch (format)
                {
                    case "AAC":
                        audioResult = true;
                        break;
                    case "PCM":
                        audioResult = true;
                        break;
                    case "MPEG Audio":
                        audioResult = true;
                        break;
                }
            }
            else
            {
                audioResult = true; //Video uden audio er OK
            }

            return hasAudio;
        }

        //-6 Lyd ulovlig 
        //-2 Ugyldig DLL version
        //-1 Ukendt fejl
        // 0 ingen streams
        // 1 Lyd streams er OK
        public static AudioCompressionTestvalue TestAudioKompressionByImageInfo(string filnavn)
        {
            var result = AudioCompressionTestvalue.UkendtFejl;
            var audioResult = false;
            var hasAudio = false;

            //Test if version of DLL is compatible : 3rd argument is "version of DLL tested;Your application name;Your application version"
            var mediaInfo = new MediaInfo();

            if (mediaInfo.Option("Info_Version").Length == 0)
                return AudioCompressionTestvalue.UgyldigDLLVersion;

            //!!! Mangler at returnere de korrekte tag data som der kan testes på !!!!

            mediaInfo.Open(filnavn);

            var streamCount = mediaInfo.Count_Get(StreamKind.General);

            //Hvis ingen Stream ...
            if (streamCount == 0) return AudioCompressionTestvalue.IngenStreams;

            //!!!! mangler at teste mere dybgående og at skelne mellem de enkelte streams !!!!
            for (var i = 0; i < streamCount; i++)
            {
                //Test video
                if (mediaInfo.Count_Get(StreamKind.Video, i) > 0)
                    return AudioCompressionTestvalue.LydUlovlig; //Vi vil ikke have video i en MP3 fil !!!

                //Test audio
                //if (mediaInfo.Count_Get(StreamKind.Audio, i) > 0)
                //{
                //    hasAudio = true;
                //    var format = mediaInfo.Get(StreamKind.Audio, i, "Format");
                //    var Codec = mediaInfo.Get(StreamKind.Audio, i, "Codec/String");
                //    var Version = mediaInfo.Get(StreamKind.Audio, i, "Format_Version");
                //    var Profile = mediaInfo.Get(StreamKind.Audio, i, "Format profile");

                //    switch (format)
                //    {
                //        case "AAC":
                //            audioResult = true;
                //            break;
                //        case "PCM":
                //            audioResult = true;
                //            break;
                //        case "MPEG Audio":
                //            audioResult = true;
                //            break;
                //        default:
                //            break;
                //    }
                //}
                hasAudio = TestAudio(mediaInfo, i, hasAudio, ref audioResult);
            }

            if (hasAudio == false)
            {
                result = AudioCompressionTestvalue.IngenStreams; //Samme som tidligere test "Ingen streams"
            }
            else
            {
                if (audioResult)
                    result = AudioCompressionTestvalue.LydStreamsErOK;
                else
                    result = AudioCompressionTestvalue.LydUlovlig;
            }

            return result;
        }

        /// <summary>
        ///     Simpel test om der findes mindst en´ mediastream i filen - siger intet om type, antal etc.
        /// </summary>
        /// <param name="filnavn"></param>
        /// <returns></returns>
        //-2 = Ugyldig DLL version
        //-1 = Ukendt fejl
        // 0 = ingen streams        
        // 1 = Stream 
        public ImageInfoResult TestMediaStreamByImageInfo(string filnavn)
        {
            try
            {
                //Test if version of DLL is compatible : 3rd argument is "version of DLL tested;Your application name;Your application version"
                var mediaInfo = new MediaInfo();

                if (mediaInfo.Option("Info_Version").Length == 0)
                    return ImageInfoResult.InvalidDLLVersion; //-2;

                mediaInfo.Open(filnavn);
                var streamCount = mediaInfo.Count_Get(StreamKind.General);

                //Hvis ingen Stream ...
                if (streamCount > 0) return ImageInfoResult.SomeStream; //1;
                return ImageInfoResult.NoStreams; //0;
            }
            catch (Exception x)
            {
                _log.Error(x.Message, x);
                return ImageInfoResult.UnknownError; //-1;
            }
        }

        private static bool TestVideo(MediaInfo mediaInfo, int i, bool hasVideo, ref bool videoResult)
        {
            if (mediaInfo.Count_Get(StreamKind.Video, i) > 0)
            {
                hasVideo = true;
                var format = mediaInfo.Get(StreamKind.Video, i, "Format");
                var codec = mediaInfo.Get(StreamKind.Video, i, "Codec/String");
                var version = mediaInfo.Get(StreamKind.Video, i, "Format_Version");
                var profile = mediaInfo.Get(StreamKind.Video, i, "Format profile");

                switch (format)
                {
                    case "AVC":
                        videoResult = true; //!!!!! VLAN fejler !!!!!!
                        break;
                    case "MPEG-4":
                        videoResult = true;
                        break;
                    case "MPEG-4 Visual":
                        videoResult = true;
                        break;
                    case "MPEG-2":
                        videoResult = true;
                        break;
                    case "MPEG Video":
                        videoResult = version == "Version 2";
                        break;
                }
            }

            return hasVideo;
        }

        /// <summary>
        ///     Tests the video kompression by image information.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>A <see cref="VideoCompressionTestvalue" /> that</returns>
        public static VideoCompressionTestvalue TestVideoKompressionByImageInfo(string filename)
        {
            var result = VideoCompressionTestvalue.UnknownError; //Ukendt fejl
            var videoResult = false;
            var audioResult = false;
            var hasAudio = false;
            var hasVideo = false;

            //Test if version of DLL is compatible : 3rd argument is "version of DLL tested;Your application name;Your application version"
            var mediaInfo = new MediaInfo();

            if (mediaInfo.Option("Info_Version").Length == 0)
                return VideoCompressionTestvalue.InvalidDLLVersion;

            //!!! Mangler at returnere de korrekte tag data som der kan testes på !!!!

            mediaInfo.Open(filename);

            var streamCount = mediaInfo.Count_Get(StreamKind.General);

            //Hvis ingen Stream ...
            if (streamCount == 0) return VideoCompressionTestvalue.NoStreams;

            //!!!! mangler at teste mere dybgående og at skelne mellem de enkelte streams !!!!
            for (var i = 0; i < streamCount; i++)
            {
                hasVideo = TestVideo(mediaInfo, i, hasVideo, ref videoResult);
                hasAudio = TestAudio(mediaInfo, i, hasAudio, ref audioResult);
            } //for (int i = 0; i < streamCount; i++)

            //Video med lyd
            if (hasAudio && hasVideo)
            {
                if (videoResult && audioResult)
                    result = VideoCompressionTestvalue.AudioAndVideoOK;
                else if ((videoResult && audioResult) == false)
                    result = VideoCompressionTestvalue.AudioAndVideoIllegal;
                else if (audioResult == false)
                    result = VideoCompressionTestvalue.AudioIllegal;
                else if (videoResult == false)
                    result = VideoCompressionTestvalue.VideoIllegal;
            }
            else //Video uden lyd
            if (hasAudio == false && hasVideo)
            {
                result = videoResult ? VideoCompressionTestvalue.AudioAndVideoOK : VideoCompressionTestvalue.VideoIllegal;
            }

            return result;
        }

        #endregion
    }
}