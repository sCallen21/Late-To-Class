using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace Late_To_Class
{
    /// <summary>
    /// Manager for holding configurable game controls
    /// </summary>
    public sealed class GameControls
    {
        private static GameControls instance = null;
        StreamReader input = null;
        public Keys jumpKey;
        public Keys duckKey;
        public Keys moveLeft;
        public Keys moveRight;


        private GameControls() { }

        /// <summary>
        /// Creates and returns a single instance of the GameControls class
        /// </summary>
        public static GameControls Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameControls();
                return instance;
            }
        }

        /// <summary>
        /// Loads in the saved control scheme from a file
        /// </summary>
        public void LoadControls()
        {
            try
            {
                input = new StreamReader("controls.txt");

                string line = null;
                while ((line = input.ReadLine()) != null)
                {
                    string[] data = line.Split(';');
                    List<char[]> hold = new List<char[]>();
                    char[] keyBind = new char[4];

                    for(int i = 0; i < 4; i++)
                    {
                     hold.Add(data[i].ToCharArray());
                    }

                    for(int i  = 0; i < 4; i++)
                    {
                        keyBind[i] = hold[i][0];
                    }        
                    jumpKey = (Keys)(int)(char.ToUpper(keyBind[0]));
                    duckKey = (Keys)(int)(char.ToUpper(keyBind[1]));
                    moveLeft = (Keys)(int)(char.ToUpper(keyBind[2]));
                    moveRight = (Keys)(int)(char.ToUpper(keyBind[3]));

                }
            }
            catch
            {

            }
            finally
            {
                if (input != null)
                {
                    input.Close();
                }
            }
        }

        public void SaveControls()
        {
            StreamWriter output = null;
            try
            {
                output = new StreamWriter("controls.txt");
                string sControls = jumpKey.ToString() + ";" + duckKey.ToString() + ";" + moveLeft.ToString() + ";" + moveRight.ToString();
                output.WriteLine(sControls);                
            }

            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            finally
            {
                if(output != null)
                {
                    output.Close();
                }
            }
        }
    }

    
}
