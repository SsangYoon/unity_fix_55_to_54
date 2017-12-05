using System;
using System.IO;
using System.Collections.Generic;

namespace UnityDowngrader
{
    class Program
    {
        static Dictionary<string, string> componentLineReference = new Dictionary<string, string>(); // reference key, actuall component key

        public static void Main(string[] args)
        {
            Console.WindowWidth = 130;
            Console.BufferWidth = 130;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("                                                 //------------------------------//");
            Console.WriteLine("                                                 //  UNITY 5.5 to 5.4 DOWNGRADER //");
            Console.WriteLine("                                                 //------------------------------//");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("Please put your 'Asset' folder in the 'Put your asset folder here' folder and make sure to backup your project before starting.");
            Console.WriteLine("Press enter to begin");
            Console.ReadLine();

            // Start converting all .prefab files
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            FindFilesRecrusively(startupPath);

            // finished all
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("DONE");

            Console.ReadLine();
        }

        /// <summary>
        /// Converts all files in this directory and sub directories
        /// </summary>
        /// <param name="dirPath"></param>
        static void FindFilesRecrusively(string dirPath)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(dirPath))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        if (f.EndsWith(".prefab"))
                            ConvertFrom55To54(f);
                    }
                    FindFilesRecrusively(d);
                }


            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Unity 5.5 to 5.4
        /// </summary>
        /// <param name="path"></param>
        static void ConvertFrom55To54(string path)
        {
            componentLineReference.Clear();
            List<string> lines = new List<string>(System.IO.File.ReadAllText(path).Split('\n'));

            // go through each line
            for (int i = 0; i < lines.Count; i++)
            {
                // ----------------------------- // 
                //         General stuff
                // ----------------------------- // 

                // serialized version 5 to 4
                if (lines[i].StartsWith("  m_PrefabInternal") && lines[i + 1] == "  serializedVersion: 5")
                {
                    lines[i + 1] = "  serializedVersion: 4";
                    continue;
                }

                // ----------------------------- // 

                // serialized version 2, delete
                if ((lines[i].Contains("tangentMode:") || lines[i].Contains("m_Curve:")) && lines[i + 1].Contains("- serializedVersion: 2") && lines[i + 2].Contains("  time:"))
                {
                    // rewrite  time:
                    lines[i + 2] = lines[i + 2].Replace("  time", "- time");

                    // remove serializedVersion: 2
                    lines.RemoveAt(i + 1);
                    continue;
                }

                // ----------------------------- // 

                // save reference component(needed later by component line fix)
                if (lines[i].StartsWith("--- !u!"))
                {
                    // get actuall key
                    string actuallKey = lines[i].Substring(7, lines[i].IndexOf(" &") - 7);

                    // get reference component key
                    string referenceComponentKey = lines[i].Substring(lines[i].IndexOf("&") + 1);

                    componentLineReference.Add(referenceComponentKey.Trim(), actuallKey);
                    continue;
                }

                // ----------------------------- // 

                //m_SelectedEditorRenderState: 3 to m_SelectedEditorRenderState: 0
                if (lines[i] == "  m_SelectedEditorRenderState: 3")
                {
                    lines[i] = "  m_SelectedWireframeHidden: 0";
                    continue;
                }

                // ----------------------------- // 

                //m_SelectedEditorRenderState: 3 to m_SelectedEditorRenderState: 0
                if (lines[i].StartsWith("  m_StaticBatchInfo:") && lines[i + 1].StartsWith("    firstSubMesh:") && lines[i + 2].StartsWith("    subMeshCount:"))
                {
                    lines.RemoveAt(i + 2);
                    lines.RemoveAt(i + 1);
                    lines[i] = "  m_SubsetIndices: ";
                    continue;
                }

                // ----------------------------- // 

                // remove SpatializePostEffects
                if (i < lines.Count - 1 && lines[i + 1].StartsWith("  SpatializePostEffects"))
                {
                    lines.RemoveAt(i + 1);
                    continue;
                }

                // ----------------------------- // 
                //       Particles system
                // ----------------------------- // 

                // particle system
                if (lines[i].Contains("ParticleSystem:") && lines[i + 5].Contains("serializedVersion: 5"))
                {
                    lines[i + 5] = lines[i + 5].Replace("5", "4");
                    continue;
                }

                // reverse move with transform
                if (lines[i].Contains("moveWithTransform: 0"))
                {
                    lines[i] = lines[i].Replace("0", "1");
                    continue;
                }
                else if (lines[i].Contains("moveWithTransform: 1"))
                {
                    lines[i] = lines[i].Replace("1", "0");
                    continue;
                }


                // initial module
                if (lines[i].Contains("InitialModule:") && lines[i + 1].Contains("serializedVersion: 3"))
                {
                    lines[i + 1] = lines[i + 1].Replace("3", "2");
                    continue;
                }

                // gravity modifier
                if (lines[i].Contains("gravityModifier:") && lines[i + 1].Contains("scalar: "))
                {
                    // get value from next line
                    string value = lines[i + 1].Substring(lines[i + 1].IndexOf(": ") + 2).Trim();
                    lines[i] = "    gravityModifier: " + value;
                    RemoveFromTo(lines, i + 1, "  ShapeModule");
                    continue;
                }

                // Remove noise module
                if (lines[i].Contains("NoiseModule:"))
                {
                    RemoveFromTo(lines, i, "  SizeBySpeedModule:");
                    continue;
                }

                // ShapeModule
                if (lines[i].Contains("ShapeModule:") && lines[i + 1].Contains("serializedVersion: 3"))
                {
                    lines[i + 1] = lines[i + 1].Replace("3", "2");
                    continue;
                }

                //randomDirectionAmount to randomDirection
                if (lines[i].Contains("randomDirectionAmount:"))
                {
                    lines[i] = lines[i].Replace("randomDirectionAmount", "randomDirection");
                    continue;
                }

                // EmissionModule
                if (lines[i].Contains("EmissionModule:") && lines[i + 2].Contains("serializedVersion: 3") && lines[i + 3].Contains("rateOverTime"))
                {
                    lines[i + 2] = lines[i + 2].Replace("3", "2");
                    lines[i + 3] = "    m_Type: 0";
                    lines.Insert(i + 4, "    rate:");
                    continue;
                }

                // SubModule - note : Completly resets the module. Fix this later
                if (lines[i].Contains("SubModule:") && lines[i + 1].Contains("serializedVersion: 2") && lines[i + 2].Contains("enabled:"))
                {
                    lines.Insert(i + 3, "    subEmitterDeath1: {fileID: 0}");
                    lines.Insert(i + 3, "    subEmitterDeath: {fileID: 0}");
                    lines.Insert(i + 3, "    subEmitterCollision1: {fileID: 0}");
                    lines.Insert(i + 3, "    subEmitterCollision: {fileID: 0}");
                    lines.Insert(i + 3, "    subEmitterBirth1: {fileID: 0}");
                    lines.Insert(i + 3, "    subEmitterBirth: {fileID: 0}");
                    lines.RemoveAt(i + 1);
                    continue;
                }

                // particle system renderer
                if (lines[i].Contains("ParticleSystemRenderer:") && lines[i + 1].Contains("serializedVersion: 2"))
                {
                    // remove empty material
                    if (lines[i + 14].Contains("- {fileID: 0}"))
                        lines.RemoveAt(i + 14);

                    // remove serialization version 2
                    lines.RemoveAt(i + 1);
                    continue;
                }
            }

            // initial rewrite complete. now do a second pass and replace all component lines
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("  - component: {fileID:"))
                {
                    // get reference key in line
                    int startingIndex = lines[i].IndexOf("fileID: ") + 8;
                    string referenceKey = lines[i].Substring(startingIndex, lines[i].IndexOf("}") - startingIndex);

                    // replace line by value saved in dictionary
                    lines[i] = lines[i].Replace("component", componentLineReference[referenceKey.Trim()]);
                }
            }

            // save lines back to the file
            System.IO.File.WriteAllLines(path, lines);
            Console.WriteLine("Finished downgrading : " + path);
        }

        /// <summary>
        /// Removes a range of elements in a list from startIndex to a line that contains the value in "targetTo" parameter
        /// </summary>
        static void RemoveFromTo(List<string> lines, int startIndex, string targetTo, bool removeTargetTo = false)
        {
            //lines [startIndex].Remove (startIndex + 1);
            int endIndex = startIndex + 1;
            while (!lines[endIndex].Contains(targetTo))
                endIndex++;

            if (removeTargetTo)
                endIndex++;

            lines.RemoveRange(startIndex, endIndex - startIndex);
        }
    }
}