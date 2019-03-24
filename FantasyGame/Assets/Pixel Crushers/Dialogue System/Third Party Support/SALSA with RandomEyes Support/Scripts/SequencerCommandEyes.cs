using UnityEngine;
using System.Collections;
using CrazyMinnow.SALSA;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem.SequencerCommands {

	/// <summary>
	/// Sequencer command Eyes(target, [subject], [customShape|random], [duration|true|false])
	/// 
	/// - target: The GameObject to look at. You may want to specify a child
	/// GameObject located at the head of the target character. Default: listener.
	/// - subject: The GameObject with the RandomEyes component. Default: speaker.
	/// - customShape|random: If 'random', sets to random. If blank, leaves untouched.
	/// Otherwise sets to the specified shape; if the shape starts with 'group ', it 
	/// will use a shape group.
	/// - duration: The duration to play the custom shape. Default: 0=forever.
	/// 	- Alternatively, true or false to set the status of a shape group.
	/// </summary>
	public class SequencerCommandEyes : SequencerCommand {

		public void Start() {
			var target = GetSubject(0, Sequencer.Listener);
			var subject = GetSubject(1, Sequencer.Speaker);
			var customShape = GetParameter(2);
			var duration = GetParameterAsFloat(3);
			var hasStatusParameter = string.Equals(GetParameter(3), "true", System.StringComparison.OrdinalIgnoreCase) || string.Equals(GetParameter(3), "false", System.StringComparison.OrdinalIgnoreCase);
			var status = hasStatusParameter ? string.Equals(GetParameter(3), "true", System.StringComparison.OrdinalIgnoreCase) : false;
			var eyes = (subject != null) ? subject.GetComponent<RandomEyes3D>() : null;
			if (eyes == null) {
				if (DialogueDebug.LogWarnings) Debug.LogWarning(string.Format("{0}: Sequencer: Eyes({1},{2},{3},{4}) command: No RandomEyes component found on {2}.", new System.Object[] { DialogueDebug.Prefix, GetParameter(0), (subject != null) ? subject.name : GetParameter(1), customShape, duration }));
			} else if (target == null) {
				if (DialogueDebug.LogWarnings) Debug.LogWarning(string.Format("{0}: Sequencer: Eyes({1},{2},{3},{4}) command: Target '{1}' not found.", new System.Object[] { DialogueDebug.Prefix, GetParameter(0), subject.name, customShape, duration }));
			} else {
				if (DialogueDebug.LogInfo) Debug.Log(string.Format("{0}: Sequencer: Eyes({1},{2},{3},{4}) command: Setting RandomEyes.", new System.Object[] { DialogueDebug.Prefix, GetParameter(0), subject.name, customShape, duration }), subject);
				eyes.SetLookTarget(target.gameObject);
				eyes.enabled = true;
				if (string.Equals(customShape, "random")) {
					eyes.SetCustomShapeRandom(true);
				} else if (!string.IsNullOrEmpty(customShape)) {

					if (customShape.StartsWith("group ", System.StringComparison.OrdinalIgnoreCase)) {

						// Groups:
						var groupName = customShape.Substring("group ".Length);
						if (hasStatusParameter) {
							eyes.SetGroup(groupName, status);
						} else {
							eyes.SetGroup(groupName, duration);
						}

					} else {

						// Regular shapes:
						if (Tools.ApproximatelyZero(duration)) {
							eyes.SetCustomShape(customShape);
						} else {
							eyes.SetCustomShape(customShape, duration);
						}
					}
				}
			}
			Stop();
		}
		
	}

}
