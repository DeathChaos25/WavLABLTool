# WavLABLTool

## Overview
This is a simple program designed to add cue markers (labels) to a WAV file. This was explicitly designed with wwise usage in mind, more specifically, to add new voicelines to Fire Emblem: Engage and have working lipsync/expression file labels.

## Requirements
- **.NET 8** (Required to run the executable)

## Usage Instructions

``` Simply drag and drop a .wav file into the program and it'll create an "Output" folder with the new modified .wav file inside of it.  ```

## Label Input Options  
- No Label File: If no .txt file is provided, the tool automatically uses the filename (without extension) as the label. This is the default mode and this is enough to have working lipsync.  
- With Label File: If a .txt file (with the same name as the WAV file) exists, each line in the .txt file is treated as a separate label. This is a second use case where you can add additional labels to the wav file for additional features (such as Expression labels).
