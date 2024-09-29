using General;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "ScreenSetting", menuName = "SO/ScreenSetting")]
    public class SScreenSetting : ScriptableObject
    {
        [SerializeField, Header("�X�N���[���ݒ�")]
        private SerializedScreenSetting serializedScreenSetting;
        internal SerializedScreenSetting SerializedScreenSetting => serializedScreenSetting;
    }
}