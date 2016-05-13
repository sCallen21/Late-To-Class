using Microsoft.Xna.Framework;
using System;
 using Microsoft.Xna.Framework.Content;
 using Microsoft.Xna.Framework.Graphics;
 using System.Collections.Generic;
 using System.Linq;
 using System.Text;
 
//Chris Banks
 namespace Late_To_Class
 {
     class NPCGenerator
     {
         #region Variables
         ContentManager Content;
         NPC person;
         Texture2D builder;
         List<NPC> people = new List<NPC>();
#endregion
 
         #region Helpers
         public List<NPC> CreateSpawn(ContentManager Content)
         {
             this.Content = Content;
             builder = Content.Load<Texture2D>("NPCS");
             Random rand = new Random();
             int nClusterSize = rand.Next(1,6);
             for(int i = 0; i < nClusterSize; i++)
             {
                 person = new NPC();
                 Create(person);
                 people.Add(person);
             }
 
             return people;
         }
 
         public void Create(NPC human)
        {
            Random rand = new Random();
            int nHairStyle = rand.Next(0, 5);
            int nBodyStle = rand.Next(0, 4);
            int red = rand.Next(0, 256);
            int green = rand.Next(0, 256);
            int blue = rand.Next(0, 256);
            human.SetContent(nBodyStle, nHairStyle, red, green, blue, builder);
        }
#endregion
     }
 }
