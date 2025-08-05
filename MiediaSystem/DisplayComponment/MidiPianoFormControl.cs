using System;
using System.Collections.Generic;
using System.Linq;

using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;

namespace NagaisoraFramework.Miedia
{
    public class MidiPianoFormControl : CommMonoScriptObject
    {
		public MidiControl MidiControl;

		public MidiPianoForm[] MidiPianoForms;

		public IEnumerable<Note> notes;

		public IEnumerable<TrackChunk> MidiChannels;

		public IDictionary<int, List<Note>> Notekeys;

		public void Start()
		{
			Notekeys = new Dictionary<int, List<Note>>();
		}

		public void FixedUpdate()
		{
			Notekeys.Clear();

			foreach (var form in MidiPianoForms)
			{
				if (!Notekeys.ContainsKey(form.ID))
				{
					Notekeys.Add(form.ID, new List<Note>());
				}
			}

			if (MidiControl is null || MidiControl.MidiNotes is null || MidiControl.Playback is null)
			{
				goto keyupdate;
			}

			notes = MidiControl.MidiNotes.AtTime((MetricTimeSpan)MidiControl.CurrentTime(TimeSpanType.Metric), MidiControl.Playback.TempoMap, LengthedObjectPart.Entire);

			if (notes == null || notes.Count() == 0)
			{
				goto keyupdate;
			}

			foreach (var note in notes)
			{
				if (!Notekeys.ContainsKey(note.Channel))
				{
					Notekeys.Add(note.Channel, new List<Note>());
				}

				Notekeys[note.Channel].Add(note);
			}

			keyupdate:
			KeyUpdate();
		}

		public void KeyUpdate()
		{
			foreach (MidiPianoForm midiPianoForm in MidiPianoForms)
			{
				if (!Notekeys.ContainsKey(midiPianoForm.ID))
				{
					continue;
				}

				midiPianoForm.notes = Notekeys[midiPianoForm.ID].ToArray();
			}
		}
	}
}