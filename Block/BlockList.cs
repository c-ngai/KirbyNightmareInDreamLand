using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{ 
    public class BlockList
    {
        private static BlockList instance;

        // Using a singleton to avoid lots of global variables that will get deleted later. 

        List<Sprite> blockList;
        int currentBlock;
        int firstBlock = 0;
        int lastBlock = 12;


        private BlockList()
        {
        }

        // Singleton instance method. Allows other files to access this classes variables. 
        public static BlockList Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BlockList();
                }
                return instance;
            }
        }

        // Singleton constructor must be empty, so this does constructor stuff.
        public void setBlockList(List<Sprite> myBlocks)
        {
            blockList = myBlocks;
            currentBlock = firstBlock;
            lastBlock = myBlocks.Count;
        }

        // If we aren't at the end of the list, view the next. If we are, go to the beginning.
        public void viewNext()
        {
            if (currentBlock < lastBlock)
            {
                currentBlock++;
            }
            else
            {
                currentBlock = firstBlock;
            }
        }

        // If we aren't at the front of the list, view the previous. If we are, go to the end. 
        public void viewPrevious()
        {
            if (currentBlock > firstBlock)
            {
                currentBlock--;
            }
            else
            {
                currentBlock = lastBlock;
            }
        }

        // Draws the current block. 
        public void Draw(Vector2 location)
        {
            if (currentBlock >= 0 && currentBlock < blockList.Count)
            {
                blockList[currentBlock].Draw(location);
            }
        }

        public void Update()
        {
            // do nothing 
        }

    }

}