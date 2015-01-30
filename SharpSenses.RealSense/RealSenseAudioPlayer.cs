using System.IO;
using System.Media;

namespace SharpSenses.RealSense {
    public static class RealSenseAudioPlayer {
        // Conversion extracted from the sample voice_synthesis.cs (RSSDK\framework\CSharp\voice_synthesis.cs)
        // To understand a little bit more about the WAV file format, acess the following links: 
        //     - http://www-mmsp.ece.mcgill.ca/documents/AudioFormats/WAVE/WAVE.html
        //     - http://www.sonicspot.com/guide/wavefiles.html
        public static void Play(PXCMAudio.AudioData audioData, PXCMAudio.AudioInfo audioInfo) {
            using (var memoryStream = new MemoryStream()) {
                using (var bw = new BinaryWriter(memoryStream)) {
                    bw.Write(0x46464952);  // chunkIdRiff:'FFIR'
                    bw.Write(0);           // chunkDataSizeRiff
                    bw.Write(0x45564157);  // riffType:'EVAW'
                    bw.Write(0x20746d66);  // chunkIdFmt:' tmf'
                    bw.Write(0x12);        // chunkDataSizeFmt
                    bw.Write((short)1);         // compressionCode
                    bw.Write((short)audioInfo.nchannels);  // numberOfChannels
                    bw.Write(audioInfo.sampleRate);   // sampleRate
                    bw.Write(audioInfo.sampleRate * 2 * audioInfo.nchannels); // averageBytesPerSecond
                    bw.Write((short)(audioInfo.nchannels * 2));   // blockAlign
                    bw.Write((short)16);        // significantBitsPerSample
                    bw.Write((short)0);         // extraFormatSize
                    bw.Write(0x61746164);  // chunkIdData:'atad'
                    bw.Write(0);           // chunkIdSizeData

                    bw.Write(audioData.ToByteArray());
                    long pos = bw.Seek(0, SeekOrigin.Current);
                    bw.Seek(0x2a, SeekOrigin.Begin); // chunkDataSizeData
                    bw.Write((int)(pos - 46));
                    bw.Seek(0x04, SeekOrigin.Begin); // chunkDataSizeRiff
                    bw.Write((int)(pos - 8));
                    bw.Seek(0, SeekOrigin.Begin);

                    using (var soundPlayer = new SoundPlayer(memoryStream)) {
                        soundPlayer.PlaySync();
                    }
                }
            }
        }
    }
}