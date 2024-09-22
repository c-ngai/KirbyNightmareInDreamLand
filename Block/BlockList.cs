using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame.Block
{ 

    public class BlockList
    {

        List<Texture2D> blockList;
        int currentBlock;
        int firstBlock = 0;
        int lastBlock;

        public BlockList(List<Texture2D> myBlocks)
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
        }
            if (currentBlock == firstBlock)
            {
                currentBlock = lastBlock;
            }
            else
            {
                currentBlock--;
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

        public void viewNext()
        {
            if (currentBlock == lastBlock)
            {
                currentBlock = firstBlock;
            }
            else
            {
                currentBlock++; 
            }
        }

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

        }

    }

}