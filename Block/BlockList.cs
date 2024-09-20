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
            if (currentBlock == firstBlock)
            {
                currentBlock = lastBlock;
            }
            else
            {
                currentBlock--;
            }

        }

    }

}