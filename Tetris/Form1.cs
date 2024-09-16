using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Text;
using System.IO;


namespace Tetris
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 
        /// nes tetris by Lucas Vanderwielen
        /// 
        /// use enter to select anything
        /// use x to rotate shapes
        /// use the arrow keys to move around the piece and to move through the menu
        /// 
        /// in order to use the score saving system a txt file must be created
        /// and you will have to add in the path to where it is saved.
        /// 
        /// TO ENSURE PROPER SAVE DATA THE FILE MUST BE FORMATTED AFTER CREATION COPY THESE EXACT LINES
        /// INTO THE txt FILE
        /// 
        /// 000000
        /// 000000
        /// 000000
        /// 00
        /// 00
        /// -----
        /// -----
        /// -----
        /// 
        /// </summary>
      
        
        Random rnd = new Random();
        int[,,] grid = new int[10,21,2];
        int[,,] screenSave = new int[10, 21, 2];
        Image[,] tileImages = new Image[10, 21];
        Image[,] imageLogs = new Image[10, 4];

        

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
       

       
        int rotationState = 0;
        Label linesCleared = new Label();

        int lines = 0;

        int level = 0;
        int levelSave = 0;
        int levelSelected = 0;

        int score = 0;

        bool lose = false;

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private PrivateFontCollection fonts = new PrivateFontCollection();

        Font myFont;


        //statistics grids

        Image[,] tShape = new Image[3, 3];
        Image[,] rL = new Image[3, 3];
        Image[,] lZig = new Image[3, 3];
        Image[,] box = new Image[3, 3];
        Image[,] rZig = new Image[3, 3];
        Image[,] ll = new Image[3, 3];
        Image[,] stick = new Image[4, 3];

        //shapes statistics array
        int[] statistics = new int[7];


        bool startScreenBool = true;
        bool levelSelectionScreen = false;


        int levelSelectedx = 0;
        int levelSelectedy = 0; 

        //highscores
        string[] highscores = new string[6];


              //  string winDir = System.Environment.GetEnvironmentVariable("\\Lucas Vanderwielen");

        bool nameSelectionScreen = false;
        int highScoreChange = 0;

        int[,] names = new int[3, 5];

        
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

            for (int i = 0; i < 3; i++)
            {
                for (int x = 0; x < 5; x++)
                {
                    names[i, x] = 45;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                highscores[i] = "000000";
            }
            for (int i = 3; i < 6; i++)
            {
                highscores[i] = "00";
            }

          

            InitializeComponent();

            


            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
           
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
            label18.Visible = false;
            label19.Visible = false;
            label20.Visible = false;
            label21.Visible = false;
            
            label23.Visible = false;
            label24.Visible = false;
            label25.Visible = false;
            label26.Visible = false;
            label27.Visible = false;
            label28.Visible = false;
            label29.Visible = false;
            label30.Visible = false;
            label31.Visible = false;
            label32.Visible = false;
            label33.Visible = false;
            label34.Visible = false;



            statsShapes();

           
            
            

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


            label6.Font = myFont;
            label7.Font = myFont;
            label8.Font = myFont;
            label9.Font = myFont;
            label10.Font = myFont;
            label11.Font = myFont;
            label5.Font = myFont;
            label12.Font = myFont;
            label13.Font = myFont;
            label14.Font = myFont;
            label15.Font = myFont;
            label16.Font = myFont;
            label17.Font = myFont;
            label18.Font = myFont;
            label19.Font = myFont;
            label20.Font = myFont;
            label21.Font = myFont;
          
            label23.Font = myFont;
            label24.Font = myFont;
            label25.Font = myFont;
            label26.Font = myFont;
            label27.Font = myFont;
            label28.Font = myFont;
            label29.Font = myFont;
            label30.Font = myFont;
            label31.Font = myFont;
            label32.Font = myFont;
            label33.Font = myFont;
            label34.Font = myFont;
          
           





           

            label3.Text = "000000";


            readingFile();
           
        }


        private void updateHighScoreText()
        {
            label13.Text = highscores[0];
            label16.Text = highscores[1];
            label17.Text = highscores[2];
            label14.Text = highscores[3];
            label18.Text = highscores[4];
            label15.Text = highscores[5];
        }
        
        private void statsShapes()
        {
            //setting up statistics grids
            tShape[0, 1] = imageSelector(1);
            tShape[1, 1] = imageSelector(1);
            tShape[2, 1] = imageSelector(1);
            tShape[1, 2] = imageSelector(1);

            rL[2, 2] = imageSelector(2);
            rL[0, 1] = imageSelector(2);
            rL[1, 1] = imageSelector(2);
            rL[2, 1] = imageSelector(2);

            lZig[0, 1] = imageSelector(5);
            lZig[1, 1] = imageSelector(5);
            lZig[2, 2] = imageSelector(5);
            lZig[1, 2] = imageSelector(5);

            box[0, 1] = imageSelector(4);
            box[1, 1] = imageSelector(4);
            box[1, 2] = imageSelector(4);
            box[0, 2] = imageSelector(4);

            rZig[1, 1] = imageSelector(6);
            rZig[2, 1] = imageSelector(6);
            rZig[1, 2] = imageSelector(6);
            rZig[0, 2] = imageSelector(6);

            ll[0, 2] = imageSelector(3);
            ll[0, 1] = imageSelector(3);
            ll[1, 1] = imageSelector(3);
            ll[2, 1] = imageSelector(3);

            stick[0, 1] = imageSelector(0);
            stick[1, 1] = imageSelector(0);
            stick[2, 1] = imageSelector(0);
            stick[3, 1] = imageSelector(0);
        }

        

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
            
            //background
            e.Graphics.DrawImage(Resource1.BackgroundV2, 0, 0);

            //size of squares
            int size = 24;

            //stats boards
            int[] initalxStats = { 60,60,60,70,60,60,50 };
            int[] initalyStats = { 180, 240 + 10, 300 + 10, 360 + 10, 420 + 10, 480 + 10, 540 + 20 };
           
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (tShape[x, y] != null)
                    {
                        e.Graphics.DrawImage(tShape[x, y], x * size + initalxStats[0] + x, y * size + initalyStats[0] + y);
                    }
                    if (rL[x, y] != null)
                    {
                        e.Graphics.DrawImage(rL[x, y], x * size + initalxStats[0] + x, y * size + initalyStats[1] + y);
                    }
                    if (lZig[x, y] != null)
                    {
                        e.Graphics.DrawImage(lZig[x, y], x * size + initalxStats[0] + x, y * size + initalyStats[2] + y);
                    }
                    if (box[x, y] != null)
                    {
                        e.Graphics.DrawImage(box[x, y], x * size + initalxStats[3] + x, y * size + initalyStats[3] + y);
                    }
                    if (rZig[x, y] != null)
                    {
                        e.Graphics.DrawImage(rZig[x, y], x * size + initalxStats[0] + x, y * size + initalyStats[4] + y);
                    }
                    if (ll[x, y] != null)
                    {
                        e.Graphics.DrawImage(ll[x, y], x * size + initalxStats[0] + x, y * size + initalyStats[5] + y);
                    }
                }
            }
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (stick[x, y] != null)
                    {
                        e.Graphics.DrawImage(stick[x, y], x * size + initalxStats[6] + x, y * size + initalyStats[6] + y);
                    }
                }
            }

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
            


            if (levelSelectionScreen)
            {
                label12.Location = new Point(163 + levelSelectedx * 47 + 4 * levelSelectedx, 227 + levelSelectedy * 47 + 2 * levelSelectedy);
                
                e.Graphics.DrawImage(Resource1.LevelSelectionScreen, -150, 0, 1100, 657);


                e.Graphics.FillRectangle(Brushes.Yellow, 161 + levelSelectedx * 47 + 4 * levelSelectedx, 222 + levelSelectedy * 47 + 2 * levelSelectedy, 47, 46);

            }
            else
            {
                label12.Location = new Point(-100, -100);
            }



            if (startScreenBool)
            {
                e.Graphics.DrawImage(Resource1.StartScreen, -90, 0, 998, 657);
            }


            if (nameSelectionScreen)
            {
                e.Graphics.DrawImage(Resource1.nameSelection, -178, 0, 1158, 657);
               
                e.Graphics.FillRectangle(Brushes.DarkOrange, 215 + 30*selectedRow, 442 + 45*(highScoreChange <= 0 ? 0 : (highScoreChange -1)), 40, 30);
               
            }

           

        }

        private void levelChange()
        {
            
                statsShapes();
                

                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 20; y++)
                    {
                        if (tileImages[x, y] == imageLogs[level - 1, 0])
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
            if (!lose && !startScreenBool && !levelSelectionScreen)

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
                score += 40 * (truelevel + 1);
            }
            else if (numberOfLines == 2)
            {
                score += 100 * (truelevel + 1);
            }
            else if (numberOfLines == 3)
            {
                score += 300 * (truelevel + 1);
            }
            else if (numberOfLines == 4)
            {
                score += 1200 * (truelevel + 1);
            }

            

            label3.Text = score.ToString();
            label4.Text = highscores[0];
            for (int i = 0; i < 6-score.ToString().Length; i++)
            {
                label3.Text = "0" + label3.Text;
              
            }

           
        }
        private void readingFile()
        {
            if (!saving)
            {
                StreamReader reader = new StreamReader("C:\\Scores\\HighScores.txt");
                int counting = 0;
                string readLine = "";
                try
                {
                    do
                    {
                        if (counting < 6)
                        {
                            highscores[counting] = reader.ReadLine();

                        }
                        else
                        {
                            readLine = reader.ReadLine();
                            for (int i = 0; i < 5; i++)
                            {
                                names[counting - 6, i] = System.Convert.ToInt16((char)readLine[i]);


                            }
                        }
                        counting += 1;



                    }
                    while (reader.Peek() != -1);
                }
                catch
                {

                }
                finally
                {
                    reader.Close();
                }

                updateNamelabel();
                updateHighScoreText();
            }
        }

        bool saving = false;
        private void writeToFile()
        {
            highScoreChange = 0;
            saving = true;
            if (score >= System.Convert.ToInt32(highscores[0]))
            {
                highScoreChange = 1;
                for (int i = 0; i < 5; i++)
                {
                    names[2,i] = names[1,i];
                    names[1,i] = names[0,i];
                    names[0, i] = 45;
                }
                highscores[2] = highscores[1];
                highscores[1] = highscores[0];
                highscores[0] = score.ToString();

                highscores[5] = highscores[4];
                highscores[4] = highscores[3];
                for (int i = 0; i < 6 - score.ToString().Length; i++)
                {
                    highscores[0] = "0" + highscores[0];
                }
                if (level >= 10)
                {
                    
                    highscores[3] = level.ToString();
                }
                else
                {
                    highscores[3] = "0" + level.ToString();
                }
            }
            else if (score >= System.Convert.ToInt32(highscores[1]))
            {
                highScoreChange = 2;
                for (int i = 0; i < 5; i++)
                {
                    names[2, i] = names[1, i];
                    names[1, i] = 45;

                }

                highscores[2] = highscores[1];
                highscores[1] = score.ToString();

                highscores[5] = highscores[4];
                for (int i = 0; i < 6 - score.ToString().Length; i++)
                {
                    highscores[1] = "0" + highscores[1];
                }
                if (level >= 10)
                {
                    
                    highscores[4] = level.ToString();
                }
                else
                {
                    highscores[4] = "0" + level.ToString();
                }
            }
            else if (score >= System.Convert.ToInt32(highscores[2]))
            {
               
                highScoreChange = 3;

                for (int i = 0; i < 5; i++)
                {
                   
                    names[2, i] = 45;

                }

                highscores[2] = score.ToString();

                for (int i = 0; i < 6 - score.ToString().Length; i++)
                {
                    highscores[2] = "0" + highscores[2];
                }

                if (level >= 10)
                {
                    highscores[5] = level.ToString();
                }
                else
                {
                    highscores[5] = "0" + level.ToString();
                }
            }

                int writerLines = 9;
                

            StreamWriter writer = new StreamWriter("C:\\Scores\\HighScores.txt");
            for (int i = 0; i < writerLines; i++)
            {
                if (i < 6)
                {
                    writer.WriteLine(highscores[i]);
                }
                else
                {
                    writer.WriteLine(((char)names[i-6, 0]).ToString() + ((char)names[i-6, 1]).ToString() + ((char)names[i-6, 2]).ToString() + ((char)names[i-6, 3]).ToString() + ((char)names[i-6, 4]).ToString());
                }
            }
            writer.Close();

            updateHighScoreText();
            updateNamelabel();
            saving = false;

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
            if (!startScreenBool && !levelSelectionScreen)
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
            }
            Refresh();

        }

        private void moveRight()
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
                // checks if the shapes is against a right wall
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

        private void moveLeft()
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

        int selectedRow = 0;
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (nameSelectionScreen)
            {
                if (e.KeyCode == Keys.Up)
                {
                    if (names[highScoreChange - 1, selectedRow] == 45)
                    {
                        names[highScoreChange - 1, selectedRow] = 65;
                    }
                    else if (names[highScoreChange - 1, selectedRow] < 90)
                    {
                        names[highScoreChange - 1, selectedRow] += 1;
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (names[highScoreChange - 1, selectedRow] == 45)
                    {
                        names[highScoreChange - 1, selectedRow] = 90;
                    }
                    else if (names[highScoreChange - 1, selectedRow] > 65)
                    {
                        names[highScoreChange - 1, selectedRow] -= 1;
                    }
                }
                else if (e.KeyCode == Keys.Left)
                {
                    if (selectedRow > 0)
                    {
                        selectedRow -= 1;
                        changeBackColor(false);
                    }
                }
                else if (e.KeyCode == Keys.Right)
                {
                    if (selectedRow < 4)
                    {
                        selectedRow += 1;
                        changeBackColor(false);
                    }
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    score = -1;
                    writeToFile();
                    reset();
                    nameSelectionScreen = false;
                 

                    highScoreChange = 0;
                    levelSelectionScreen = true;
                    changeBackColor(true);
                 
                   
                }
                updateNamelabel();
            }
            else
            {
                if (!startScreenBool && !goingDown)
                {

                    if ((e.KeyCode == Keys.Down || e.KeyCode == Keys.K))
                    {
                        if (!levelSelectionScreen)
                        {

                            goingDown = true;
                        }
                        else
                        {
                            if (levelSelectedy < 1)
                            {
                                levelSelectedy += 1;
                            }
                        }
                    }

                    else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.L)
                    {
                        if (!levelSelectionScreen)
                        {
                            moveRight();
                        }
                        else
                        {
                            if (levelSelectedx < 4)
                            {
                                levelSelectedx += 1;
                            }
                        }
                    }
                    else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.J)
                    {
                        if (!levelSelectionScreen)
                        {
                            moveLeft();
                        }
                        else
                        {
                            if (levelSelectedx > 0)
                            {
                                levelSelectedx -= 1;
                            }
                        }
                    }
                    else if (e.KeyCode == Keys.X)
                    {
                        if (!levelSelectionScreen)
                        {
                            rotate(newShape);
                        }


                    }
                    else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.I)
                    {
                        if (levelSelectionScreen)
                        {

                            if (levelSelectedy > 0)
                            {
                                levelSelectedy -= 1;
                            }

                        }
                    }
                    else if (e.KeyCode == Keys.Enter)
                    {




                        if (levelSelectionScreen)
                        {
                            levelSelectionScreen = false;
                            
                            changeLevelSelectedBox();
                         


                            if (level != 0)
                            {
                                levelChange();
                                updateLevel();
                                statsShapes();
                            }

                            lose = false;
                            newShape = rnd.Next(0, 7);
                            createShape();


                            nameLabels(false);
                            toggleLabelViews(true);
                            scoreLabelViews(false);
                        }

                        if (lose)
                        {

                            if (highScoreChange != 0)
                            {
                              
                                changeBackColor(false);
                                nameSelectionScreen = true;
                                nameLabels(true);
                                toggleLabelViews(false);
                                scoreLabelViews(true);
                               
                            }
                            else
                            {
                               
                                toggleLabelViews(false);
                                scoreLabelViews(true);
                            }

                            lose = false;
                            reset();
                           
                            
                        }




                    }
                    changeLevelSelectedBox();

                }
                else if (e.KeyCode == Keys.Enter)
                {
                    startScreenBool = false;
                    levelSelectionScreen = true;
                    scoreLabelViews(true);
                    nameLabels(true);
                    updateNamelabel();



                }
            }


            Refresh();


        }

        private void changeBackColor(bool wipe)
        {
            label19.BackColor = Color.Black;
            label20.BackColor = Color.Black;
            label21.BackColor = Color.Black;
            label23.BackColor = Color.Black;
            label24.BackColor = Color.Black;
            label25.BackColor = Color.Black;
            label26.BackColor = Color.Black;
            label27.BackColor = Color.Black;
            label28.BackColor = Color.Black;
            label29.BackColor = Color.Black;
            label30.BackColor = Color.Black; 
            label31.BackColor = Color.Black;
            label32.BackColor = Color.Black;
            label33.BackColor = Color.Black;
            label34.BackColor = Color.Black;
            if (!wipe)
            {
                if (highScoreChange - 1 == 0)
                {
                    if (selectedRow == 0)
                    {
                        label19.BackColor = Color.Transparent;
                    }
                    else if (selectedRow == 1)
                    {
                        label20.BackColor = Color.Transparent;
                    }
                    else if (selectedRow == 2)
                    {
                        label21.BackColor = Color.Transparent;
                    }
                    else if (selectedRow == 3)
                    {
                        label23.BackColor = Color.Transparent;
                    }
                    else if (selectedRow == 4)
                    {
                        label24.BackColor = Color.Transparent;
                    }

                }
                else if (highScoreChange - 1 == 1)
                {
                    if (selectedRow == 0)
                    {
                        label29.BackColor = Color.Transparent;
                    }
                    else if (selectedRow == 1)
                    {
                        label28.BackColor = Color.Transparent;
                    }
                    else if (selectedRow == 2)
                    {
                        label27.BackColor = Color.Transparent;
                    }
                    else if (selectedRow == 3)
                    {
                        label26.BackColor = Color.Transparent;
                    }
                    else if (selectedRow == 4)
                    {
                        label25.BackColor = Color.Transparent;
                    }

                }
                else if (highScoreChange - 1 == 2)
                {
                    if (selectedRow == 0)
                    {
                        label34.BackColor = Color.Transparent;
                    }
                    else if (selectedRow == 1)
                    {
                        label33.BackColor = Color.Transparent;
                    }
                    else if (selectedRow == 2)
                    {
                        label32.BackColor = Color.Transparent;
                    }
                    else if (selectedRow == 3)
                    {
                        label31.BackColor = Color.Transparent;
                    }
                    else if (selectedRow == 4)
                    {
                        label30.BackColor = Color.Transparent;
                    }

                }
            }
        }

        private void updateNamelabel()
        {
           
            label19.Text = ((Char)(names[0, 0])).ToString();
            label20.Text = ((Char)names[0, 1]).ToString();
            label21.Text = ((Char)names[0, 2]).ToString();
            label23.Text = ((Char)names[0, 3]).ToString();
            label24.Text = ((Char)names[0, 4]).ToString();

            label29.Text = ((Char)names[1, 0]).ToString();
            label28.Text = ((Char)names[1, 1]).ToString();
            label27.Text = ((Char)names[1, 2]).ToString();
            label26.Text = ((Char)names[1, 3]).ToString();
            label25.Text = ((Char)names[1, 4]).ToString();


            label34.Text = ((Char)names[2, 0]).ToString();
            label33.Text = ((Char)names[2, 1]).ToString();
            label32.Text = ((Char)names[2, 2]).ToString();
            label31.Text = ((Char)names[2, 3]).ToString();
            label30.Text = ((Char)names[2, 4]).ToString();
        }
        private void nameLabels(bool state)
        {
            if (state)
            {
                label19.Visible = true;
                label20.Visible = true;
                label21.Visible = true;
                
                label23.Visible = true;
                label24.Visible = true;
                label25.Visible = true;
                label26.Visible = true;
                label27.Visible = true;
                label28.Visible = true;
                label29.Visible = true;
                label30.Visible = true;
                label31.Visible = true;
                label32.Visible = true;
                label33.Visible = true;
                label34.Visible = true;
                
            }
            else
            {
                label19.Visible = false;
                label20.Visible = false;
                label21.Visible = false;
                
                label23.Visible = false;
                label24.Visible = false;
                label25.Visible = false;
                label26.Visible = false;
                label27.Visible = false;
                label28.Visible = false;
                label29.Visible = false;
                label30.Visible = false;
                label31.Visible = false;
                label32.Visible = false;
                label33.Visible = false;
                label34.Visible = false;
            }
        }
        private void scoreLabelViews(bool state)
        {
            if (state)
            {
                label13.Visible = true;
                label14.Visible = true;
                label15.Visible = true;
                label16.Visible = true;
                label17.Visible = true;
                label18.Visible = true;
            }
            else
            {
                label13.Visible = false;
                label14.Visible = false;
                label15.Visible = false;
                label16.Visible = false;
                label17.Visible = false;
                label18.Visible = false;
            }

        }
        
        private void toggleLabelViews(bool state)
        {
            if (state)
            {
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                label8.Visible = true;
                label9.Visible = true;
                label10.Visible = true;
                label11.Visible = true;
            }
            else
            {
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                label11.Visible = false;
            }

        }
       
        private void changeLevelSelectedBox()
        {
            if (!levelSelectionScreen)
            {
                levelSelected = (levelSelectedx + levelSelectedy * 5);
            }
            label12.Text = (levelSelectedx + levelSelectedy * 5).ToString();
        }
        private void loseChecker()
        {
            for (int y = 0; y < 2; y++)
            {
                for (int x = 4; x < 6; x++)
                {
                    if (grid[x, y, 0] == 1 && grid[x, y, 1] == 0)
                    {
                        if (!lose)
                        {
                            writeToFile();
                        }
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

            while (num == newShape)
            {
                num = rnd.Next(0, 7);
            }
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
                        updateLevel();
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
        int truelevel = 0;
        private void updateLevel()
        {
            if (lines >= ((levelSelected * 10)))
            {
                level = ((lines-(levelSelected*10)) / 10) + levelSelected;
                
            }
            else
            {
                level = levelSelected;
            }
            truelevel = level;

            label2.Text = level.ToString();

            if (level > 9)
            {
                level = level - 10;
            }


            
            if (level != levelSave)
            {
                if (level != 0)
                {
                    levelChange();
                    updateSpeed();
                }
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
            if (!startScreenBool)
            {
               
                statsUpdate();
                timerCount += 1;
                if (!lose)
                {
                    loseChecker();
                }
                if (timerCount % 2 == 0)
                {
                    loseScreen();
                }
                updateLevel();


                

            }
        }
        

        private void statsUpdate()
        {
            label5.Text = addingZeros(statistics[1]);
            label6.Text = addingZeros(statistics[2]);
            label7.Text = addingZeros(statistics[5]);
            label8.Text = addingZeros(statistics[4]);
            label9.Text = addingZeros(statistics[6]);
            label10.Text = addingZeros(statistics[3]);
            label11.Text = addingZeros(statistics[0]);
        }

        private string addingZeros(int num)
        {
            string added = "";
            for (int i = 0; i < 3-num.ToString().Length; i++)
            {
                added += "0";
            }
            return added + num.ToString();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Down || e.KeyCode == Keys.K)
            {

                goingDown = false;
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        
        private void DropTimer_Tick(object sender, EventArgs e)
        {
            if (goingDown)
            {
                
                moveDown();
            }
        }

        private void reset()
        {
            count = 0;
            level = 0;
            levelSave = 0;
            score = 0;
            lines = 0;
            goingDown = false;
            startScreenBool = false;
            levelSelectionScreen = true;
            num = rnd.Next(0, 7);

            for (int i = 0; i < 7; i++)
            {
                statistics[i] = 0;
            }
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
          

            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;


            statsShapes();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
