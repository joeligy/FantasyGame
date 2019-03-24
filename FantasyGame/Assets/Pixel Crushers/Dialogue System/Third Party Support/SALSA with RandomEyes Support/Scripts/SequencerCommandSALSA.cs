using UnityEngine;
using System.Collections;
using CrazyMinnow.SALSA;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    /// <summary>
    /// Sequencer command SALSA(clip, [subject], [nowait).
    /// 
    /// - clip: The path to an audio clip in a Resources folder.
    /// - subject: The GameObject with the Salsa2D/3D component. Default: speaker.
    /// - If present and 'nowait', the command doesn't wait for SALSA to finish.
    /// </summary>
    public class SequencerCommandSALSA : SequencerCommand
    {

        private Salsa3D salsa3d = null;
        private Salsa2D salsa2d = null;
        private AudioClip clip = null;
        private bool nowait = false;

        private const float SalsaFixDelay = 0.1f;
        private float startDelayRemaining = 0;

        public void Start()
        {
            clip = Resources.Load(GetParameter(0)) as AudioClip;
            var subject = GetSubject(1, Sequencer.Speaker);
            nowait = string.Equals(GetParameter(2), "nowait", System.StringComparison.OrdinalIgnoreCase);
            salsa2d = (subject != null) ? subject.GetComponent<Salsa2D>() : null;
            salsa3d = (subject != null) ? subject.GetComponent<Salsa3D>() : null;
            if (!HasSalsaComponent())
            {
                if (DialogueDebug.LogWarnings) Debug.LogWarning(string.Format("{0}: Sequencer: SALSA({1},{2},{3}) command: No Salsa2D/3D component found on {2}.", new System.Object[] { DialogueDebug.Prefix, GetParameter(0), (subject != null) ? subject.name : GetParameter(1), GetParameter(2) }));
                Stop();
            }
            else if (clip == null)
            {
                if (DialogueDebug.LogWarnings) Debug.LogWarning(string.Format("{0}: Sequencer: SALSA({1},{2},{3}) command: Audio clip not found. Is it in a Resources folder?", new System.Object[] { DialogueDebug.Prefix, GetParameter(0), subject.name, GetParameter(2) }), subject);
                Stop();
            }
            else
            {
                if (DialogueDebug.LogInfo) Debug.Log(string.Format("{0}: Sequencer: SALSA({1},{2},{3}) command: Playing audio clip.", new System.Object[] { DialogueDebug.Prefix, GetParameter(0), subject.name, GetParameter(2) }), subject);

                //--- SALSA 
                //--- Was: PlaySalsa();
                //--- Was if (nowait) Stop();
                startDelayRemaining = SalsaFixDelay;
            }
        }
            
        private bool HasSalsaComponent()
        {
            return ((salsa2d != null) || (salsa3d != null));
        }

        private bool IsSalsaTalking()
        {
            return ((salsa2d != null) && salsa2d.isTalking) ||
                ((salsa3d != null) && salsa3d.isTalking);
        }

        private void PlaySalsa()
        {
            if (salsa2d != null)
            {
                salsa2d.SetAudioClip(clip);
                salsa2d.Play();
            }
            else
            {
                salsa3d.SetAudioClip(clip);
                salsa3d.Play();
            }
        }

        private void StopSalsa()
        {
            if ((salsa2d != null) && (salsa2d.audioClip == clip)) salsa2d.Stop();
            if ((salsa3d != null) && (salsa3d.audioClip == clip)) salsa3d.Stop();
        }

        public void Update()
        {
            if (startDelayRemaining > 0)
            {
                startDelayRemaining -= Time.deltaTime;
                if (startDelayRemaining <= 0)
                {
                    PlaySalsa();
                    if (nowait) Stop();
                }
            }
            else
            {
                if (!IsSalsaTalking()) Stop();
            }
        }

        public void OnDestroy()
        {
            if (!nowait)
            {
                StopSalsa();
            }
        }

    }

}
