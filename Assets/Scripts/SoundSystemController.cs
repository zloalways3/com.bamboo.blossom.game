using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SoundSystemController : MonoBehaviour
{
    [FormerlySerializedAs("_resonanceCore")] [SerializeField] private AudioMixer _coreResonance;
    [FormerlySerializedAs("waveToggleGlyph")] [SerializeField] private Image waveSwitchIcon;
    [FormerlySerializedAs("melodyToggleGlyph")] [SerializeField] private Image melodySwitchIcon;
    [FormerlySerializedAs("pulseOnSigil")] [SerializeField] private Sprite pulseActivateGlyph;
    [FormerlySerializedAs("pulseOffSigil")] [SerializeField] private Sprite pulseDeactivateGlyph;
    [FormerlySerializedAs("saveButton")] [SerializeField] private Button btnSaveGame;

    private bool _echoPulseEnabled;
    private bool _orchestrationThread;

    void Start()
    {
        _echoPulseEnabled = PlayerPrefs.GetInt(UniversalConstants.SONIC_PULSE_SWITCH, 1) == 1;
        _orchestrationThread = PlayerPrefs.GetInt(UniversalConstants.HARMONIC_FLOW_ENABLED, 1) == 1;
        
        ConfigureAudioGrid();
        ShiftAudioPitch();
        
        btnSaveGame.onClick.AddListener(PauseMark);
    }

    public void ToggleSonicPulse()
    {
        _echoPulseEnabled = !_echoPulseEnabled;
        ConfigureAudioGrid();
    }

    public void ActivateResonanceField()
    {
        _orchestrationThread = !_orchestrationThread;
        ShiftAudioPitch();
    }

    private void ConfigureAudioGrid()
    {
        _coreResonance.SetFloat(UniversalConstants.HARMONIC_FLOW_ENABLED, _echoPulseEnabled ? 0f : -80f);
        waveSwitchIcon.sprite = _echoPulseEnabled ? pulseActivateGlyph : pulseDeactivateGlyph;
    }

    private void ShiftAudioPitch()
    {
        _coreResonance.SetFloat(UniversalConstants.SONIC_PULSE_SWITCH, _orchestrationThread ? 0f : -80f);
        melodySwitchIcon.sprite = _orchestrationThread ? pulseActivateGlyph : pulseDeactivateGlyph;
    }
    
    public void PauseMark()
    {
        PlayerPrefs.SetInt(UniversalConstants.SONIC_PULSE_SWITCH, _echoPulseEnabled ? 1 : 0);
        PlayerPrefs.SetInt(UniversalConstants.HARMONIC_FLOW_ENABLED, _orchestrationThread ? 1 : 0);
        PlayerPrefs.Save();
    }
}