using System;
using UnityEngine;
using UnityEngine.Audio;

namespace General
{
    public enum SoundType
    {
        Master,
        BGM,
        SE
    }

    public static class SoundManager
    {
        /// <summary>
        /// null�`�F�b�N�ς�
        /// SoundType.Master�͕s��
        /// </summary>
        public static void Raise(this AudioSource source, AudioClip clip, SoundType type)
        {
            if (source == null) return;
            if (clip == null) return;
            if (type is SoundType.Master) return;

            if (type is SoundType.BGM)
            {
                source.playOnAwake = false;
                source.loop = true;

                source.clip = clip;
                source.Play();
            }
            else if (type is SoundType.SE)
            {
                source.playOnAwake = false;
                source.loop = false;

                source.PlayOneShot(clip);
            }
            else throw new Exception("�����Ȏ�ނł�");
        }

        public static float BGMVolume
        {
            get => GetVolume(SoundType.BGM);
            set => SetVolume(SoundType.BGM, value);
        }

        public static float SEVolume
        {
            get => GetVolume(SoundType.SE);
            set => SetVolume(SoundType.SE, value);
        }

        private static float GetVolume(SoundType type)
        {
            if (am == null) return 0;
            am.GetFloat(type.ToParamNameString(), out float volume);
            return volume;
        }

        private static void SetVolume(SoundType type, float newVolume)
        {
            if (am == null) return;
            am.SetFloat(type.ToParamNameString(), newVolume);
        }

        private static AudioMixer am => null;  // ������

        private static string ToParamNameString(this SoundType type) => type switch
        {
            SoundType.Master => "MasterParam",
            SoundType.BGM => "BGMParam",
            SoundType.SE => "SEParam",
            _ => throw new Exception("�����Ȏ�ނł�")
        };
    }
}