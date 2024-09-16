using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Text;


namespace Tetris
{
    public partial class Form1 : Form
    {
        
        Random rnd = new Random();
        int[,,] grid = new int[10,21,2];
        int[,,] screenSave = new int[10, 21, 2];
        Image[,] tileImages = new Image[10, 21];
        Image[,] imageLogs = new Image[10, 4];

        //shapes statistics array
        int[] statistics = new int[7];

        //grid for the next up display
        int[,] nextUpGrid = new int[4, 3];
        Image[,] nextUpImages = new Image[4,3];

        //used for rotation
        Point[] selectedShape = new Point[4];

        //current shape
        int newShape = 0;
        //next shape
        int num = 0;
        //inital x,y of the next up display
        int initalNextUpX = 600;
        int initalNextUpY = 330;

        //for manually going down
        bool goingDown = false;
        bool goingDownSaftey = false;

       
        int rotationState = 0;
        Label linesCleared = new Label();

        int lines = 0;

        int level = 0;
        int levelSave = 0;

        int score = 0;

        bool lose = false;

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private PrivateFontCollection fonts = new PrivateFontCollection();

        Font myFont;
       
        public Form1()
        {
            imageLogs[0, 0] = Resource1.lvl_1_1;
            imageLogs[0, 1] = Resource1.lvl1_2;
            imageLogs[0, 2] = Resource1.lvl1_3;
            imageLogs[1, 0] = Resource1.lvl_2_1;
            imageLogs[1, 1] = Resource1.lvl_2_2;
            imageLogs[1, 2] = Resource1.lvl2_3;
            imageLogs[2, 0] = Resource1.lvl3_1;
            imageLogs[2, 1] = Resource1.lvl3_2;
            imageLogs[2, 2] = Resource1.lvl3_3;
            imageLogs[3, 0] = Resource1.lvl4_1;
            imageLogs[3, 1] = Resource1.lvl4_2;
            imageLogs[3, 2] = Resource1.lvl4_3;
            imageLogs[4, 0] = Resource1.lvl5_1;
            imageLogs[4, 1] = Resource1.lvl5_2;
            imageLogs[4, 2] = Resource1.lvl5_3;
            imageLogs[5, 0] = Resource1.lvl6_1;
            imageLogs[5, 1] = Resource1.lvl6_2;
            imageLogs[5, 2] = Resource1.lvl6_3;
            imageLogs[6, 0] = Resource1.lvl7_1;
            imageLogs[6, 1] = Resource1.lvl7_2;
            imageLogs[6, 2] = Resource1.lvl7_3;
            imageLogs[7, 0] = Resource1.lvl8_1;
            imageLogs[7, 1] = Resource1.lvl8_2;
            imageLogs[7, 2] = Resource1.lvl8_3;
            imageLogs[8, 0] = Resource1.lvl9_1;
            imageLogs[8, 1] = Resource1.lvl9_2;
            imageLogs[8, 2] = Resource1.lvl9_3;
            imageLogs[9, 0] = Resource1.lvl10_1;
            imageLogs[9, 1] = Resource1.lvl10_2;
            imageLogs[9, 2] = Resource1.lvl10_3;
            imageLogs[0, 3] = Resource1.lvl1_4;
            imageLogs[1, 3] = Resource1.lvl2_4;
            imageLogs[2, 3] = Resource1.lvl3_4;
            imageLogs[3, 3] = Resource1.lvl4_4;
            imageLogs[4, 3] = Resource1.lvl5_4;
            imageLogs[5, 3] = Resource1.lvl6_4;
            imageLogs[6, 3] = Resource1.lvl7_4;
            imageLogs[7, 3] = Resource1.lvl8_4;
            imageLogs[8, 3] = Resource1.lvl9_4;
            imageLogs[9, 3] = Resource1.lvl10_4;



            num = rnd.Next(0, 7);
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    tileImages[x, y] = Resource1.BackgroundTileImage;
                    for (int i = 0; i <= 1; i++)
                    {
                        grid[x, y, i] = 0;
                    }
                }
            }
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 3; y++)
                {

                    nextUpImages[x, y] = Resource1.BackgroundTileImage;
                    

                }
            }

            for (int i = 0; i < 10; i++)
            {
                grid[i, 20, 0] = 1;

            }
            InitializeComponent();
            
            newShape = rnd.Next(0, 7);
            createShape();

            byte[] fontData = Resource1.Emulogic_zrEw;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Resource1.Emulogic_zrEw.Length);
            AddFontMemResourceEx(fontPtr, (uint)Resource1.Emulogic_zrEw.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);
           

            myFont = new Font(fonts.Families[0], 20.0F);
            label1.Font = myFont;
            label2.Font = myFont;
            label3.Font = myFont;
            label4.Font = myFont;


            label3.Text = "000000";
        }

        

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //background
            e.Graphics.DrawImage(Resource1.BackgroundV2, 0, 0);

            //size of squares
            int size = 24;

            //intial x,y of the tetris board
            int initalx = 297;
            int initaly = 126;
            
            //draws the main board
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    
                        // e.Graphics.FillRectangle(Brushes.White, x * size + initalx + x, y * size + initaly + y, size, size);
                        e.Graphics.DrawImage(tileImages[x,y], x * size + initalx + x, y * size + initaly + y);
                       
                        
                    

                }
            }

            //draws the next up display
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (nextUpGrid[x, y] == 1)
                    {
                       // e.Graphics.FillRectangle(Brushes.White, x * size + initalNextUpX + x, y * size + initalNextUpY + y, size, size);
                        e.Graphics.DrawImage(nextUpImages[x, y], x * size + initalNextUpX + x, y * size + initalNextUpY + y);
                    }

                }
            }

        }

        private void levelChange()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                   if (tileImages[x,y] == imageLogs[level-1,0])
                    {
                        tileImages[x, y] = imageLogs[level, 0];
                    }
                   else if (tileImages[x, y] == imageLogs[level - 1, 1])
                    {
                        tileImages[x, y] = imageLogs[level, 1];
                    }
                    else if (tileImages[x, y] == imageLogs[level - 1, 2])
                    {
                        tileImages[x, y] = imageLogs[level, 2];
                    }

                }
            }

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (nextUpGrid[x, y] == 1)
                    {
                        nextUpImages[x, y] = imageSelector(num);
                    }

                }
            }
        }


        private void BlockTimer_Tick(object sender, EventArgs e)
        {
            if (!lose)
            {
                moveDown();
            }
           
        }

        private void clearingLines()
        {
            int numberOfLines = 0;
            int[] lowestY = new int[10];
            bool flagged = false;
            for (int y = 0; y < 20; y++)
            {
                int count = 0;
                for (int x = 0; x < 10; x++)
                {
                    if (grid[x,y,0] == 1)
                    {
                        count += 1;
                    }
                }
                if (count == 10)
                {
                    lines += 1;
                    lowestY[numberOfLines] = y;
                    numberOfLines += 1;
                   
                    flagged = true;

                    
                    
                    
                }
            }

            if (flagged)
            {
                for (int i = 0; i <= 4; i++)
                {
                    for (int repeat = 0; repeat < numberOfLines; repeat++)
                    {
                        tileImages[4 - i, lowestY[repeat]] = Resource1.BackgroundTileImage;


                        tileImages[i + 5, lowestY[repeat]] = Resource1.BackgroundTileImage;
                    }

                    Refresh();

                    System.Threading.Thread.Sleep(30);

                }
                System.Threading.Thread.Sleep(50);
                for (int repeat = 0; repeat < numberOfLines; repeat++)
                {
                    recursiveDown(lowestY[repeat]);
                }
            }

           

            //scoring
            label1.Text = lines.ToString();
            if (numberOfLines == 1)
            {
                score += 40 * (level + 1);
            }
            else if (numberOfLines == 2)
            {
                score += 100 * (level + 1);
            }
            else if (numberOfLines == 3)
            {
                score += 300 * (level + 1);
            }
            else if (numberOfLines == 4)
            {
                score += 1200 * (level + 1);
            }
            
            label3.Text = score.ToString();
            for (int i = 0; i < 6-score.ToString().Length; i++)
            {
                label3.Text = "0" + label3.Text;
            }
        }
        private int recursiveDown(int y)
        {
            if (y == 0)
            {
                return 0;
            }
            else
            {
                for (int i = 0; i <= 4; i++)
                {
                   
                    tileImages[4-i, y] = tileImages[4-i, y - 1];

                    
                    tileImages[i+5, y] = tileImages[i+5, y - 1];

                    grid[i + 5, y, 0] = grid[i + 5, y - 1, 0];
                     grid[4-i, y, 0] = grid[4-i, y - 1, 0];

                }
                
            }
            return recursiveDown(y-1);
        }
        

        private void moveDown()
        {
            //this is used for collison detections, gets flagged when we are colliding with something
            bool stopper = false;



            // index the whole screen to see if that shape can be moved down
            for (int yi = 18; yi >= 0; yi--)
            {
                for (int xi = 0; xi < 10; xi++)
                {
                    if (grid[xi, yi, 1] == 1 && grid[xi, yi + 1, 0] == 1 && grid[xi, yi + 1, 1] == 0)
                    {
                        stopper = true;
                    }

                }
            }
            if (!stopper)
            {
                for (int y = 19; y >= 0; y--)
                {


                    for (int x = 0; x < 10; x++)
                    {

                        //checks if spot is selected
                        if (grid[x, y, 1] == 1)
                        {
                            //makes sure the box below is empty
                            if (grid[x, y + 1, 0] == 0)
                            {
                                //switches the box with the one below
                                grid[x, y + 1, 0] = 1;
                                grid[x, y, 0] = 0;
                                grid[x, y + 1, 1] = 1;
                                grid[x, y, 1] = 0;
                                tileImages[x, y + 1] = tileImages[x, y];
                                tileImages[x, y] = Resource1.BackgroundTileImage;
                            }
                            else
                            {


                                createShape();

                            }
                        }


                    }

                }
            }
            else
            {


                createShape();
            }

            Refresh();

        }


        

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //moves shape right
                if (e.KeyCode == Keys.Right || e.KeyCode == Keys.L)
                {
                //this is used for collison detections, gets flagged when we are colliding with something
                bool stopper = false;

                    // index the whole screen to see if that shape can be moved right
                    for (int yi = 19; yi >= 0; yi--)
                    {
                        for (int xi = 0; xi < 9; xi++)
                        {
                        
                            if (grid[xi, yi, 1] == 1 && grid[xi + 1, yi, 0] == 1 && grid[xi + 1, yi, 1] == 0)
                            {
                                stopper = true;
                            }

                        }
                    }

                    if (!stopper)
                    {
                        /// checks if the shapes is against a right wall
                        bool wallHitR = false;
                        for (int i = 0; i < 20; i++)
                        {
                            if (grid[9, i, 1] == 1)
                            {
                                wallHitR = true;
                            }
                        }

                        for (int y = 19; y >= 0; y--)
                        {
                            for (int x = 9; x >= 0; x--)
                            {
                                //checking if we are on edge of board, so that it wont crash
                                int addedValue = x + 1;
                                if (x + 1 == 10)
                                {
                                    addedValue = x;
                                }

                                if (grid[x, y, 1] == 1 && grid[addedValue, y, 1] == 0 && !wallHitR)
                                {

                                    //switches the box with the one on the right
                                    grid[addedValue, y, 0] = 1;
                                    grid[x, y, 0] = 0;
                                    grid[addedValue, y, 1] = 1;
                                    grid[x, y, 1] = 0;
                                tileImages[addedValue, y] = imageSelector(newShape);
                                tileImages[x, y] = Resource1.BackgroundTileImage;
                            }
                            }
                        }
                    }
                }
                else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.J)
                {
                //move shape left

                //this is used for collison detections, gets flagged when we are colliding with something
                bool stopper = false;

                    // index the whole screen to see if that shape can be moved left
                    for (int yi = 19; yi >= 0; yi--)
                    {
                        for (int xi = 1; xi < 10; xi++)
                        {
                        //makes sure it can be moved left
                            if (grid[xi, yi, 1] == 1 && grid[xi - 1, yi, 0] == 1 && grid[xi - 1, yi, 1] == 0)
                            {
                                stopper = true;
                            }

                        }
                    }

                    if (!stopper)
                    {
                    //makes sure were not on a wall so it doesnt crash
                        bool wallHitL = false;
                        for (int i = 0; i < 20; i++)
                        {
                            if (grid[0, i, 1] == 1)
                            {
                                wallHitL = true;
                            }
                        }
                        for (int y = 19; y >= 0; y--)
                        {
                            for (int x = 0; x < 10; x++)
                            {

                                //checking if we are on edge of board, so it doesnt crash
                                int addedValue = x - 1;
                                if (x - 1 == -1)
                                {
                                    addedValue = x;

                                }



                                if (grid[x, y, 1] == 1 && grid[addedValue, y, 1] == 0 && !wallHitL)
                                {
                                    grid[addedValue, y, 0] = 1;
                                    grid[x, y, 0] = 0;
                                    grid[addedValue, y, 1] = 1;
                                    grid[x, y, 1] = 0;
                                tileImages[addedValue, y] = imageSelector(newShape);
                                tileImages[x, y] = Resource1.BackgroundTileImage;
                                     
                                }
                            }
                        }
                    }
                }
                else if (e.KeyCode == Keys.X)
                {
                    rotate(newShape);

                }
                else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.K)
                {
                    goingDown = true;
                }
            Refresh();
        }
        private void loseChecker()
        {
            for (int y = 0; y < 2; y++)
            {
                for (int x = 4; x < 6; x++)
                {
                    if (grid[x, y, 0] == 1 && grid[x, y, 1] == 0)
                    {
                        lose = true;
                    }
                }
            }
        }
        private void rotate(int shape)
        {
            //these are used to compare the amount of tiles used up before and after a rotation because if the numbers are different
            //then the rotation clipped into another shape so we revert to before the rotation
            int count = 0;
            int cellCount = 0;


            //saves the current screen into a save variable
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        screenSave[x, y, i] = grid[x, y, i];
                    }

                }
            }

            //counts how many tiles are not empty
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (grid[x, y, 0] == 1)
                    {
                        cellCount += 1;

                    }

                }
            }


            //add to rotation state 
            addToRotationState(rotationState);

            //using try becasue it would be too many checks to make it not crash
            try
            {
                //index screen and finds where the selected shapes are
                for (int y = 0; y < 20; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        if (grid[x, y, 1] == 1)
                        {
                            selectedShape[count] = new Point(x, y);
                            count += 1;
                            tileImages[x, y] = Resource1.BackgroundTileImage;

                        }

                    }
                }   
           
                if (shape == 0)
                {
                    //stick shape rotate
                    if (rotationState == 1)
                    {

                    
                            grid[selectedShape[0].X + 1, selectedShape[0].Y - 2, 0] = 1;
                            grid[selectedShape[0].X + 1, selectedShape[0].Y - 2, 1] = 1;
                            grid[selectedShape[0].X, selectedShape[0].Y, 0] = 0;
                            grid[selectedShape[0].X, selectedShape[0].Y, 1] = 0;



                            grid[selectedShape[2].X - 1, selectedShape[2].Y - 1, 0] = 1;
                            grid[selectedShape[2].X - 1, selectedShape[2].Y - 1, 1] = 1;
                            grid[selectedShape[2].X, selectedShape[2].Y, 0] = 0;
                            grid[selectedShape[2].X, selectedShape[2].Y, 1] = 0;

                            grid[selectedShape[3].X - 2, selectedShape[3].Y + 1, 0] = 1;
                            grid[selectedShape[3].X - 2, selectedShape[3].Y + 1, 1] = 1;
                            grid[selectedShape[3].X, selectedShape[3].Y, 0] = 0;
                            grid[selectedShape[3].X, selectedShape[3].Y, 1] = 0;
                    


                    }
                    else if (rotationState == 2)
                    {

                    
                            grid[selectedShape[0].X - 1, selectedShape[0].Y + 1, 0] = 1;
                            grid[selectedShape[0].X - 1, selectedShape[0].Y + 1, 1] = 1;
                            grid[selectedShape[0].X, selectedShape[0].Y, 0] = 0;
                            grid[selectedShape[0].X, selectedShape[0].Y, 1] = 0;



                            grid[selectedShape[2].X + 1, selectedShape[2].Y - 1, 0] = 1;
                            grid[selectedShape[2].X + 1, selectedShape[2].Y - 1, 1] = 1;
                            grid[selectedShape[2].X, selectedShape[2].Y, 0] = 0;
                            grid[selectedShape[2].X, selectedShape[2].Y, 1] = 0;

                            grid[selectedShape[3].X + 2, selectedShape[3].Y - 2, 0] = 1;
                            grid[selectedShape[3].X + 2, selectedShape[3].Y - 2, 1] = 1;
                            grid[selectedShape[3].X, selectedShape[3].Y, 0] = 0;
                            grid[selectedShape[3].X, selectedShape[3].Y, 1] = 0;
                    

                    }
                    else if (rotationState == 3)
                    {

                            grid[selectedShape[0].X + 2, selectedShape[0].Y - 1, 0] = 1;
                            grid[selectedShape[0].X + 2, selectedShape[0].Y - 1, 1] = 1;
                            grid[selectedShape[0].X, selectedShape[0].Y, 0] = 0;
                            grid[selectedShape[0].X, selectedShape[0].Y, 1] = 0;

                            grid[selectedShape[1].X + 1, selectedShape[1].Y + 1, 0] = 1;
                            grid[selectedShape[1].X + 1, selectedShape[1].Y + 1, 1] = 1;
                            grid[selectedShape[1].X, selectedShape[1].Y, 0] = 0;
                            grid[selectedShape[1].X, selectedShape[1].Y, 1] = 0;



                            grid[selectedShape[3].X - 1, selectedShape[3].Y + 2, 0] = 1;
                            grid[selectedShape[3].X - 1, selectedShape[3].Y + 2, 1] = 1;
                            grid[selectedShape[3].X, selectedShape[3].Y, 0] = 0;
                            grid[selectedShape[3].X, selectedShape[3].Y, 1] = 0;
                    

                    }
                    else if (rotationState == 0)
                    {

                    
                            grid[selectedShape[0].X + 1, selectedShape[0].Y + 2, 0] = 1;
                            grid[selectedShape[0].X + 1, selectedShape[0].Y + 2, 1] = 1;
                            grid[selectedShape[0].X, selectedShape[0].Y, 0] = 0;
                            grid[selectedShape[0].X, selectedShape[0].Y, 1] = 0;

                            grid[selectedShape[1].X - 1, selectedShape[1].Y + 1, 0] = 1;
                            grid[selectedShape[1].X - 1, selectedShape[1].Y + 1, 1] = 1;
                            grid[selectedShape[1].X, selectedShape[1].Y, 0] = 0;
                            grid[selectedShape[1].X, selectedShape[1].Y, 1] = 0;



                            grid[selectedShape[3].X - 2, selectedShape[3].Y - 1, 0] = 1;
                            grid[selectedShape[3].X - 2, selectedShape[3].Y - 1, 1] = 1;
                            grid[selectedShape[3].X, selectedShape[3].Y, 0] = 0;
                            grid[selectedShape[3].X, selectedShape[3].Y, 1] = 0;
                    

                    }
                }
                else if (shape == 1)
                {

                    //t shape rotate
                    if (rotationState == 1)
                    {


                        grid[selectedShape[2].X - 1, selectedShape[2].Y - 1, 0] = 1;
                        grid[selectedShape[2].X - 1, selectedShape[2].Y - 1, 1] = 1;
                        grid[selectedShape[2].X, selectedShape[2].Y, 0] = 0;
                        grid[selectedShape[2].X, selectedShape[2].Y, 1] = 0;


                    }
                    else if (rotationState == 2)
                    {


                        grid[selectedShape[3].X + 1, selectedShape[3].Y - 1, 0] = 1;
                        grid[selectedShape[3].X + 1, selectedShape[3].Y - 1, 1] = 1;
                        grid[selectedShape[3].X, selectedShape[3].Y, 0] = 0;
                        grid[selectedShape[3].X, selectedShape[3].Y, 1] = 0;


                    }
                    else if (rotationState == 3)
                    {


                        grid[selectedShape[1].X + 1, selectedShape[1].Y + 1, 0] = 1;
                        grid[selectedShape[1].X + 1, selectedShape[1].Y + 1, 1] = 1;
                        grid[selectedShape[1].X, selectedShape[1].Y, 0] = 0;
                        grid[selectedShape[1].X, selectedShape[1].Y, 1] = 0;


                    }
                    else if (rotationState == 0)
                    {


                        grid[selectedShape[0].X - 1, selectedShape[0].Y + 1, 0] = 1;
                        grid[selectedShape[0].X - 1, selectedShape[0].Y + 1, 1] = 1;
                        grid[selectedShape[0].X, selectedShape[0].Y, 0] = 0;
                        grid[selectedShape[0].X, selectedShape[0].Y, 1] = 0;


                    }
                }
                else if (shape == 2)
                {
                    //left l shape rotate
                    if (rotationState == 1)
                    {
                    
                            grid[selectedShape[0].X + 2, selectedShape[0].Y, 0] = 1;
                            grid[selectedShape[0].X + 2, selectedShape[0].Y, 1] = 1;
                            grid[selectedShape[0].X, selectedShape[0].Y, 0] = 0;
                            grid[selectedShape[0].X, selectedShape[0].Y, 1] = 0;

                            grid[selectedShape[1].X + 1, selectedShape[1].Y - 1, 0] = 1;
                            grid[selectedShape[1].X + 1, selectedShape[1].Y - 1, 1] = 1;
                            grid[selectedShape[1].X, selectedShape[1].Y, 0] = 0;
                            grid[selectedShape[1].X, selectedShape[1].Y, 1] = 0;



                            grid[selectedShape[3].X - 1, selectedShape[3].Y + 1, 0] = 1;
                            grid[selectedShape[3].X - 1, selectedShape[3].Y + 1, 1] = 1;
                            grid[selectedShape[3].X, selectedShape[3].Y, 0] = 0;
                            grid[selectedShape[3].X, selectedShape[3].Y, 1] = 0;
                    
                    }
                    else if (rotationState == 2)
                    {
                        grid[selectedShape[0].X + 1, selectedShape[0].Y + 1, 0] = 1;
                        grid[selectedShape[0].X + 1, selectedShape[0].Y + 1, 1] = 1;
                        grid[selectedShape[0].X, selectedShape[0].Y, 0] = 0;
                        grid[selectedShape[0].X, selectedShape[0].Y, 1] = 0;

                        grid[selectedShape[1].X, selectedShape[1].Y + 2, 0] = 1;
                        grid[selectedShape[1].X, selectedShape[1].Y + 2, 1] = 1;
                        grid[selectedShape[1].X, selectedShape[1].Y, 0] = 0;
                        grid[selectedShape[1].X, selectedShape[1].Y, 1] = 0;



                        grid[selectedShape[3].X - 1, selectedShape[3].Y - 1, 0] = 1;
                        grid[selectedShape[3].X - 1, selectedShape[3].Y - 1, 1] = 1;
                        grid[selectedShape[3].X, selectedShape[3].Y, 0] = 0;
                        grid[selectedShape[3].X, selectedShape[3].Y, 1] = 0;

                    }
                    else if (rotationState == 3)
                    {
                        grid[selectedShape[0].X + 1, selectedShape[0].Y - 1, 0] = 1;
                        grid[selectedShape[0].X + 1, selectedShape[0].Y - 1, 1] = 1;
                        grid[selectedShape[0].X, selectedShape[0].Y, 0] = 0;
                        grid[selectedShape[0].X, selectedShape[0].Y, 1] = 0;

                        grid[selectedShape[2].X - 1, selectedShape[2].Y + 1, 0] = 1;
                        grid[selectedShape[2].X - 1, selectedShape[2].Y + 1, 1] = 1;
                        grid[selectedShape[2].X, selectedShape[2].Y, 0] = 0;
                        grid[selectedShape[2].X, selectedShape[2].Y, 1] = 0;



                        grid[selectedShape[3].X - 2, selectedShape[3].Y, 0] = 1;
                        grid[selectedShape[3].X - 2, selectedShape[3].Y, 1] = 1;
                        grid[selectedShape[3].X, selectedShape[3].Y, 0] = 0;
                        grid[selectedShape[3].X, selectedShape[3].Y, 1] = 0;
                    }
                    else if (rotationState == 0)
                    {
                        grid[selectedShape[0].X + 1, selectedShape[0].Y + 1, 0] = 1;
                        grid[selectedShape[0].X + 1, selectedShape[0].Y + 1, 1] = 1;
                        grid[selectedShape[0].X, selectedShape[0].Y, 0] = 0;
                        grid[selectedShape[0].X, selectedShape[0].Y, 1] = 0;

                        grid[selectedShape[2].X, selectedShape[2].Y - 1, 0] = 1;
                        grid[selectedShape[2].X, selectedShape[2].Y - 1, 1] = 1;
                        grid[selectedShape[2].X, selectedShape[2].Y, 0] = 0;
                        grid[selectedShape[2].X, selectedShape[2].Y, 1] = 0;



                        grid[selectedShape[3].X - 1, selectedShape[3].Y - 2, 0] = 1;
                        grid[selectedShape[3].X - 1, selectedShape[3].Y - 2, 1] = 1;
                        grid[selectedShape[3].X, selectedShape[3].Y, 0] = 0;
                        grid[selectedShape[3].X, selectedShape[3].Y, 1] = 0;
                    }
                }
                else if (shape == 3)
                {
                    //right l shape rotate
                    if (rotationState == 1)
                    {
                        grid[selectedShape[0].X - 1, selectedShape[0].Y, 0] = 1;
                        grid[selectedShape[0].X - 1, selectedShape[0].Y, 1] = 1;
                        grid[selectedShape[0].X, selectedShape[0].Y, 0] = 0;
                        grid[selectedShape[0].X, selectedShape[0].Y, 1] = 0;

                        grid[selectedShape[1].X + 1, selectedShape[1].Y + 1, 0] = 1;
                        grid[selectedShape[1].X + 1, selectedShape[1].Y + 1, 1] = 1;
                        grid[selectedShape[1].X, selectedShape[1].Y, 0] = 0;
                        grid[selectedShape[1].X, selectedShape[1].Y, 1] = 0;



                        grid[selectedShape[3].X, selectedShape[3].Y + 1, 0] = 1;
                        grid[selectedShape[3].X, selectedShape[3].Y + 1, 1] = 1;
                        grid[selectedShape[3].X, selectedShape[3].Y, 0] = 0;
                        grid[selectedShape[3].X, selectedShape[3].Y, 1] = 0;
                    }
                    else if (rotationState == 2)
                    {
                        grid[selectedShape[0].X + 1, selectedShape[0].Y + 1, 0] = 1;
                        grid[selectedShape[0].X + 1, selectedShape[0].Y + 1, 1] = 1;
                        grid[selectedShape[0].X, selectedShape[0].Y, 0] = 0;
                        grid[selectedShape[0].X, selectedShape[0].Y, 1] = 0;

                        grid[selectedShape[2].X - 1, selectedShape[2].Y - 1, 0] = 1;
                        grid[selectedShape[2].X - 1, selectedShape[2].Y - 1, 1] = 1;
                        grid[selectedShape[2].X, selectedShape[2].Y, 0] = 0;
                        grid[selectedShape[2].X, selectedShape[2].Y, 1] = 0;



                        grid[selectedShape[3].X - 2, selectedShape[3].Y, 0] = 1;
                        grid[selectedShape[3].X - 2, selectedShape[3].Y, 1] = 1;
                        grid[selectedShape[3].X, selectedShape[3].Y, 0] = 0;
                        grid[selectedShape[3].X, selectedShape[3].Y, 1] = 0;

                    }
                    else if (rotationState == 3)
                    {
                        grid[selectedShape[0].X + 1, selectedShape[0].Y - 1, 0] = 1;
                        grid[selectedShape[0].X + 1, selectedShape[0].Y - 1, 1] = 1;
                        grid[selectedShape[0].X, selectedShape[0].Y, 0] = 0;
                        grid[selectedShape[0].X, selectedShape[0].Y, 1] = 0;

                        grid[selectedShape[2].X - 1, selectedShape[2].Y + 1, 0] = 1;
                        grid[selectedShape[2].X - 1, selectedShape[2].Y + 1, 1] = 1;
                        grid[selectedShape[2].X, selectedShape[2].Y, 0] = 0;
                        grid[selectedShape[2].X, selectedShape[2].Y, 1] = 0;



                        grid[selectedShape[3].X, selectedShape[3].Y - 2, 0] = 1;
                        grid[selectedShape[3].X, selectedShape[3].Y - 2, 1] = 1;
                        grid[selectedShape[3].X, selectedShape[3].Y, 0] = 0;
                        grid[selectedShape[3].X, selectedShape[3].Y, 1] = 0;
                    }
                    else if (rotationState == 0)
                    {
                        grid[selectedShape[0].X + 2, selectedShape[0].Y, 0] = 1;
                        grid[selectedShape[0].X + 2, selectedShape[0].Y, 1] = 1;
                        grid[selectedShape[0].X, selectedShape[0].Y, 0] = 0;
                        grid[selectedShape[0].X, selectedShape[0].Y, 1] = 0;

                        grid[selectedShape[1].X + 1, selectedShape[1].Y + 1, 0] = 1;
                        grid[selectedShape[1].X + 1, selectedShape[1].Y + 1, 1] = 1;
                        grid[selectedShape[1].X, selectedShape[1].Y, 0] = 0;
                        grid[selectedShape[1].X, selectedShape[1].Y, 1] = 0;



                        grid[selectedShape[3].X - 1, selectedShape[3].Y - 1, 0] = 1;
                        grid[selectedShape[3].X - 1, selectedShape[3].Y - 1, 1] = 1;
                        grid[selectedShape[3].X, selectedShape[3].Y, 0] = 0;
                        grid[selectedShape[3].X, selectedShape[3].Y, 1] = 0;
                    }
                }
                else if (shape == 5)
                {
                    //zig left shape rotate
                    if (rotationState == 1 || rotationState == 3)
                    {
                        grid[selectedShape[3].X, selectedShape[3].Y - 2, 0] = 1;
                        grid[selectedShape[3].X, selectedShape[3].Y - 2, 1] = 1;
                        grid[selectedShape[3].X, selectedShape[3].Y, 0] = 0;
                        grid[selectedShape[3].X, selectedShape[3].Y, 1] = 0;

                        grid[selectedShape[0].X + 2, selectedShape[0].Y, 0] = 1;
                        grid[selectedShape[0].X + 2, selectedShape[0].Y, 1] = 1;
                        grid[selectedShape[0].X, selectedShape[0].Y, 0] = 0;
                        grid[selectedShape[0].X, selectedShape[0].Y, 1] = 0;





                        
                    }
                    else if (rotationState == 2 || rotationState == 0)
                    {
                        grid[selectedShape[0].X - 2, selectedShape[0].Y + 1, 0] = 1;
                        grid[selectedShape[0].X - 2, selectedShape[0].Y + 1, 1] = 1;
                        grid[selectedShape[0].X, selectedShape[0].Y, 0] = 0;
                        grid[selectedShape[0].X, selectedShape[0].Y, 1] = 0;





                        grid[selectedShape[2].X, selectedShape[2].Y + 1, 0] = 1;
                        grid[selectedShape[2].X, selectedShape[2].Y + 1, 1] = 1;
                        grid[selectedShape[2].X, selectedShape[2].Y, 0] = 0;
                        grid[selectedShape[2].X, selectedShape[2].Y, 1] = 0;

                    }

                }
                else if (shape == 6)
                {
                    //zig right shape rotate
                    if (rotationState == 1 || rotationState == 3)
                    {
                        grid[selectedShape[3].X, selectedShape[3].Y - 2, 0] = 1;
                        grid[selectedShape[3].X, selectedShape[3].Y - 2, 1] = 1;
                        grid[selectedShape[3].X, selectedShape[3].Y, 0] = 0;
                        grid[selectedShape[3].X, selectedShape[3].Y, 1] = 0;

                        grid[selectedShape[2].X + 2, selectedShape[2].Y, 0] = 1;
                        grid[selectedShape[2].X + 2, selectedShape[2].Y, 1] = 1;
                        grid[selectedShape[2].X, selectedShape[2].Y, 0] = 0;
                        grid[selectedShape[2].X, selectedShape[2].Y, 1] = 0;

                        
                    }
                    else if (rotationState == 2 || rotationState == 0)
                    {
                        grid[selectedShape[3].X - 2, selectedShape[3].Y, 0] = 1;
                        grid[selectedShape[3].X - 2, selectedShape[3].Y, 1] = 1;
                        grid[selectedShape[3].X, selectedShape[3].Y, 0] = 0;
                        grid[selectedShape[3].X, selectedShape[3].Y, 1] = 0;

                        grid[selectedShape[0].X, selectedShape[0].Y + 2, 0] = 1;
                        grid[selectedShape[0].X, selectedShape[0].Y + 2, 1] = 1;
                        grid[selectedShape[0].X, selectedShape[0].Y, 0] = 0;
                        grid[selectedShape[0].X, selectedShape[0].Y, 1] = 0;





                        

                    }

                }
                
            }
            catch (Exception e)
            {
                //subtracts from rotation state if try fails
                if (rotationState == 0)
                {
                    rotationState = 3;
                }
                else 
                {
                    rotationState -= 1;
                }
                //reverts screen to before crash
                for (int y = 0; y < 20; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            grid[x, y, i] = screenSave[x, y, i];
                        }

                    }
                }
            }

            

            //counts how many tiles are not empty 
            int currentCount = 0;
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (grid[x, y, 0] == 1)
                    {
                        currentCount += 1;
                    }

                }
            }
            

            if (currentCount != cellCount)
            {
                //sets the screen to be the saved screen if rotation clips into another shape
                for (int y = 0; y < 20; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        for(int i = 0; i < 2; i++)
                        {
                            grid[x, y, i] = screenSave[x, y, i];
                        }

                    }
                }
                //subtracts from rotationstate
                if (rotationState == 0)
                {
                    rotationState = 3;
                }
                else
                {
                    rotationState -= 1;
                }
            }
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (grid[x, y, 1] == 1)
                    {
                        tileImages[x, y] = imageSelector(shape);
                    }

                }
            }

            Refresh();




        }
        private void addToRotationState(int num)
        {
            if (num <= 2)
            {
                rotationState += 1;
            }
            else 
            {
                rotationState = 0;
            }
        }
        private void resetSelected()
        {
            clearingLines();
            goingDown = false;
            //resets the grid so that no tiles are selected
                for (int y = 0; y < 20; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        grid[x, y, 1] = 0;
                    
                    }
                }

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    nextUpGrid[x, y] = 0;

                }
            }

        }
        private void createShape()
        {

            rotationState = 0;
            resetSelected();
            
            //sets the next shape to the current shape
            newShape = num;

            //generates the next shape
            num = rnd.Next(0, 7);
            //stick next shape
            if (num == 0)
            {
                initalNextUpX = 598;
                initalNextUpY = 330;
                nextUpGrid[0, 1] = 1;
                nextUpGrid[1, 1] = 1;
                nextUpGrid[2, 1] = 1;
                nextUpGrid[3, 1] = 1;


               
            }
            //t next shape
            else if (num == 1)
            {
                initalNextUpX = 610;
                initalNextUpY = 335;

                nextUpGrid[0, 2] = 1;
                nextUpGrid[1, 2] = 1;
                nextUpGrid[2, 2] = 1;
                nextUpGrid[1, 1] = 1;

              

            }
            //left l next
            else if (num == 2)
            {
                initalNextUpX = 610;
                initalNextUpY = 345;

                nextUpGrid[0, 1] = 1;
                nextUpGrid[1, 1] = 1;
                nextUpGrid[2, 1] = 1;
                nextUpGrid[0, 0] = 1;

             

            }
            //right l next
            else if (num == 3)
            {
                initalNextUpX = 610;
                initalNextUpY = 345;
                nextUpGrid[0, 1] = 1;
                nextUpGrid[1, 1] = 1;
                nextUpGrid[2, 1] = 1;
                nextUpGrid[2, 0] = 1;


            }
            //box next
            else if (num == 4)
            {
                initalNextUpX = 625;
                initalNextUpY = 330;

                nextUpGrid[0, 1] = 1;
                nextUpGrid[1, 1] = 1;
                nextUpGrid[0, 2] = 1;
                nextUpGrid[1, 2] = 1;


            }
            //zig left next
            else if (num == 5)
            {
                initalNextUpX = 610;
                initalNextUpY = 330;

                nextUpGrid[0, 1] = 1;
                nextUpGrid[1, 1] = 1;
                nextUpGrid[1, 2] = 1;
                nextUpGrid[2, 2] = 1;


            }
            //zig right next
            else if (num == 6)
            {
                initalNextUpX = 610;
                initalNextUpY = 330;

                nextUpGrid[0, 2] = 1;
                nextUpGrid[1, 2] = 1;
                nextUpGrid[1, 1] = 1;
                nextUpGrid[2, 1] = 1;

            

            }

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (nextUpGrid[x, y] == 1)
                    {
                        nextUpImages[x, y] = imageSelector(num);
                    }

                }
            }


            statistics[newShape] += 1;
            //stick shape 
            if (newShape == 0)
            {
                grid[3, 1, 0] = 1;
                grid[4, 1, 0] = 1;
                grid[5, 1, 0] = 1;
                grid[6, 1, 0] = 1;
               

                grid[3, 1, 1] = 1;
                grid[4, 1, 1] = 1;
                grid[5, 1, 1] = 1;
                grid[6, 1, 1] = 1;

                

            }
                //t shape
            else if (newShape == 1)
            {
                grid[4, 0, 0] = 1;
                grid[5, 0, 0] = 1;
                grid[6, 0, 0] = 1;
                grid[5, 1, 0] = 1;

                grid[4, 0, 1] = 1;
                grid[5, 0, 1] = 1;
                grid[6, 0, 1] = 1;
                grid[5, 1, 1] = 1;

            }
                //left l
            else if (newShape == 2)
            {
                grid[4, 1, 0] = 1;
                grid[5, 1, 0] = 1;
                grid[6, 1, 0] = 1;
                grid[4, 0, 0] = 1;

                grid[4, 1, 1] = 1;
                grid[5, 1, 1] = 1;
                grid[6, 1, 1] = 1;
                grid[4, 0, 1] = 1;

            }
             //right l
            else if (newShape == 3)
            {
                grid[4, 1, 0] = 1;
                grid[5, 1, 0] = 1;
                grid[6, 1, 0] = 1;
                grid[6, 0, 0] = 1;

                grid[4, 1, 1] = 1;
                grid[5, 1, 1] = 1;
                grid[6, 1, 1] = 1;
                grid[6, 0, 1] = 1;

            }
            //box
            else if (newShape == 4)
            {
                grid[4, 1, 0] = 1;
                grid[5, 1, 0] = 1;
                grid[4, 0, 0] = 1;
                grid[5, 0, 0] = 1;

                grid[4, 1, 1] = 1;
                grid[5, 1, 1] = 1;
                grid[4, 0, 1] = 1;
                grid[5, 0, 1] = 1;

            }
            //zig left
            else if (newShape == 5)
            {
                grid[4, 0, 0] = 1;
                grid[5, 0, 0] = 1;
                grid[5, 1, 0] = 1;
                grid[6, 1, 0] = 1;

                grid[4, 0, 1] = 1;
                grid[5, 0, 1] = 1;
                grid[5, 1, 1] = 1;
                grid[6, 1, 1] = 1;

            }
            //zig right
            else if (newShape == 6)
            {
                grid[4, 1, 0] = 1;
                grid[5, 1, 0] = 1;
                grid[5, 0, 0] = 1;
                grid[6, 0, 0] = 1;

                grid[4, 1, 1] = 1;
                grid[5, 1, 1] = 1;
                grid[5, 0, 1] = 1;
                grid[6, 0, 1] = 1;

            }

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    if (grid[x,y,1] == 1)
                    {
                        tileImages[x, y] = imageSelector(newShape);
                    }
                }
            }

        }

        private Image imageSelector(int shape)
        {
            if (shape == 0 || shape == 1 || shape == 4)
            {
                return imageLogs[level, 0];
            }
            else if (shape == 5 || shape == 2)
            {

                return imageLogs[level, 1];
            }
            else if (shape == 3 || shape == 6)
            {

                return imageLogs[level, 2];
            }

            return Resource1.lvl_1_1;
        }
        private void updateSpeed()
        {

            if (level <= 8)
            {
                BlockTimer.Interval = System.Convert.ToInt16(((48.0 - (level * 5.0)) / 60.0) * 1000);
            }
            else if (level == 9)
            {
                BlockTimer.Interval = System.Convert.ToInt16((6 / 60.0) * 1000);
            }
            else if (level == 10 || level == 11 || level == 12)
            {
                BlockTimer.Interval = System.Convert.ToInt16((5 / 60.0) * 1000);
            }
            else if (level == 13 || level == 14 || level == 15)
            {
                BlockTimer.Interval = System.Convert.ToInt16((4 / 60.0) * 1000);
            }
            else if (level == 16 || level == 17 || level == 18)
            {
                BlockTimer.Interval = System.Convert.ToInt16((3 / 60.0) * 1000);
            }
            else if (level >= 19 && level <= 28)
            {
                BlockTimer.Interval = System.Convert.ToInt16((2 / 60.0) * 1000);
            }
            else if (level >= 29)
            {
                BlockTimer.Interval = System.Convert.ToInt16((1 / 60.0) * 1000);
            }



        }
        private void updateLevel()
        {
            level = lines / 10;
            label2.Text = level.ToString();
            if (level != levelSave)
            {
                levelChange();
                updateSpeed();
            }

            levelSave = level;
        }
        int count = 0;
        private void loseScreen()
        {
            
            if (lose)
            {

                count += 1;
                    for (int x = 0; x < 10; x++)
                    {
                    if (count <= 20)
                    {
                        tileImages[x, 20 - count] = imageLogs[level, 3];
                    }

                    }
                    
                
                Refresh();
            }
           
        }

        int timerCount = 0;
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            timerCount += 1;
            loseChecker();
            if (timerCount % 2 == 0)
            {
                loseScreen();
            }
            updateLevel();
            
           
            if (goingDown)
            {
                moveDown();
            }
            

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down || e.KeyCode == Keys.K)
            {

                goingDown = false;
            }
        }
    }
}
