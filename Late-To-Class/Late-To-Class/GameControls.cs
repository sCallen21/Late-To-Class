using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Input;

//Chris Banks
namespace Late_To_Class
{
    /// <summary>
    /// Manager for holding configurable game controls
    /// </summary>
    public sealed class GameControls
    {
        #region Variables
        static GameControls instance = null;
        StreamReader input = null;

        /// <summary>
        /// default key values for first-time loadup
        /// </summary>
        public Keys jumpKey = Keys.W;
        public Keys duckKey = Keys.S;
        public Keys moveLeft = Keys.A;
        public Keys moveRight = Keys.D;
        public Keys powerUpKey = Keys.J;

        /// <summary>
        /// array holding all the keys, ensures that no two keys are set to the same input value
        /// </summary>
        public Keys[] conflicting = new Keys[5];
        #endregion

        #region Constructor
        /// <summary>
        /// Creates and returns a single instance of the GameControls class
        /// </summary>
        public static GameControls Instance
        {
            get
            {
                if (instance == null) { instance = new GameControls(); }
                return instance;
            }
        }
        #endregion

        #region Load and Save
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
                    int[] keyBind = new int[5];

                    for (int i = 0; i < 5; i++)
                    {
                        keyBind[i] = int.Parse(data[i]);
                    }

                    jumpKey = (Keys)(keyBind[0]);
                    conflicting[0] = jumpKey;
                    duckKey = (Keys)(keyBind[1]);
                    conflicting[1] = duckKey;
                    moveLeft = (Keys)(keyBind[2]);
                    conflicting[2] = moveLeft;
                    moveRight = (Keys)(keyBind[3]);
                    conflicting[3] = moveRight;
                    powerUpKey = (Keys)(keyBind[4]);
                    conflicting[4] = powerUpKey;

                }

                if (input != null)
                {
                    input.Close();
                }
                SaveControls();
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


        /// <summary>
        /// Saves the key configurations as their integer values
        /// </summary>
        public void SaveControls()
        {
            StreamWriter output = null;
            try
            {
                output = new StreamWriter("controls.txt");
                string sControls = (int)jumpKey + ";" + (int)duckKey + ";" + (int)moveLeft + ";" + (int)moveRight + ";" + (int)powerUpKey;
                output.WriteLine(sControls);
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            finally
            {
                if (output != null)
                {
                    output.Close();
                }
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// used with the control scheme option to enable player defined controls
        /// </summary>
        /// <param name="keyType">the key to be reconfigured</param>
        /// <param name="newKey">the new key to be used</param>
        public void ConfigureControls(string keyType, Keys newKey)
        {
            bool bIsConflicting;
            switch (keyType)
            {
                case "jump":
                    bIsConflicting = false;
                    for (int i = 0; i < 5; i++)
                    {
                        if (newKey == conflicting[i])
                        {
                            bIsConflicting = true;
                        }
                    }
                    if (!bIsConflicting)
                        jumpKey = newKey;
                    break;
                case "moveLeft":
                    bIsConflicting = false;
                    for (int i = 0; i < 5; i++)
                    {
                        if (newKey == conflicting[i])
                        {
                            bIsConflicting = true;
                        }
                    }
                    if (!bIsConflicting)
                        moveLeft = newKey;
                    break;
                case "moveRight":
                    bIsConflicting = false;
                    for (int i = 0; i < 5; i++)
                    {
                        if (newKey == conflicting[i])
                        {
                            bIsConflicting = true;
                        }
                    }
                    if (!bIsConflicting)
                        moveRight = newKey;
                    break;
                case "duck":
                    bIsConflicting = false;
                    for (int i = 0; i < 5; i++)
                    {
                        if (newKey == conflicting[i])
                        {
                            bIsConflicting = true;
                        }
                    }
                    if (!bIsConflicting)
                        duckKey = newKey;
                    break;
                case "powerUp":
                    bIsConflicting = false;
                    for (int i = 0; i < 5; i++)
                    {
                        if (newKey == conflicting[i])
                        {
                            bIsConflicting = true;
                        }
                    }
                    if (!bIsConflicting)
                        powerUpKey = newKey;
                    break;
            }
        }
        #endregion
    }       
}
