using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;

namespace Pmfst_GameSDK
{
    /// <summary>
    /// -
    /// </summary>
    public partial class BGL : Form
    {
        /* ------------------- */
        #region Environment Variables

        List<Func<int>> GreenFlagScripts = new List<Func<int>>();

        /// <summary>
        /// Uvjet izvršavanja igre. Ako je <c>START == true</c> igra će se izvršavati.
        /// </summary>
        /// <example><c>START</c> se često koristi za beskonačnu petlju. Primjer metode/skripte:
        /// <code>
        /// private int MojaMetoda()
        /// {
        ///     while(START)
        ///     {
        ///       //ovdje ide kod
        ///     }
        ///     return 0;
        /// }</code>
        /// </example>
        public static bool START = true;

        //sprites
        /// <summary>
        /// Broj likova.
        /// </summary>
        public static int spriteCount = 0, soundCount = 0;

        /// <summary>
        /// Lista svih likova.
        /// </summary>
        //public static List<Sprite> allSprites = new List<Sprite>();
        public static SpriteList<Sprite> allSprites = new SpriteList<Sprite>();

        //sensing
        int mouseX, mouseY;
        Sensing sensing = new Sensing();

        //background
        List<string> backgroundImages = new List<string>();
        int backgroundImageIndex = 0;
        string ISPIS = "";

        /* Za ispis razine kreiran zaseban string, te stil istoga obradjen zasebno pomocu metode DrawTextOnScreen */
        string RAZINA = "";
       
        
        SoundPlayer[] sounds = new SoundPlayer[1000];
        TextReader[] readFiles = new StreamReader[1000];
        TextWriter[] writeFiles = new StreamWriter[1000];
        bool showSync = false;
        int loopcount;
        DateTime dt = new DateTime();
        String time;
        double lastTime, thisTime, diff;

        #endregion
        /* ------------------- */
        #region Events

        private void Draw(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            try
            {                
                foreach (Sprite sprite in allSprites)
                {                    
                    if (sprite != null)
                        if (sprite.Show == true)
                        {
                            g.DrawImage(sprite.CurrentCostume, new Rectangle(sprite.X, sprite.Y, sprite.Width, sprite.Height));
                        }
                    if (allSprites.Change)
                        break;
                }
                if (allSprites.Change)
                    allSprites.Change = false;
            }
            catch
            {
                //ako se doda sprite dok crta onda se mijenja allSprites
                //MessageBox.Show("Greška!");
            }
        }

        private void startTimer(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();
            Init();
        }

        private void updateFrameRate(object sender, EventArgs e)
        {
            updateSyncRate();
        }

        /// <summary>
        /// Crta tekst po pozornici.
        /// </summary>
        /// <param name="sender">-</param>
        /// <param name="e">-</param>
        public void DrawTextOnScreen(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            var brush = new SolidBrush(Color.WhiteSmoke);
            string text = ISPIS;
            string text2 = RAZINA;
            

            SizeF stringSize = new SizeF();
            Font stringFont = new Font("Century Gothic", 12);
            stringSize = e.Graphics.MeasureString(text, stringFont);

            SizeF stringSize2 = new SizeF();
            Font stringFont2 = new Font("Century Gothic", 10);
            stringSize2 = e.Graphics.MeasureString(text2, stringFont2);
            

            using (Font font1 = stringFont)
            {
                RectangleF rectF1 = new RectangleF((GameOptions.RightEdge / 2)-50, 20, stringSize.Width, stringSize.Height);
                e.Graphics.DrawString(text, font1, Brushes.Green, rectF1);               
            }

            using (Font font2 = stringFont2)
            {
                RectangleF rectF2 = new RectangleF((GameOptions.RightEdge / 2) - 25, 40, stringSize2.Width, stringSize2.Height);
                e.Graphics.DrawString(text2, font2, Brushes.Tan, rectF2);
            }

            
        }

        private void mouseClicked(object sender, MouseEventArgs e)
        {
            
            //sensing.MouseDown = true;
            sensing.MouseDown = true;
        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            //sensing.MouseDown = true;
            sensing.MouseDown = true;            
        }

        private void mouseUp(object sender, MouseEventArgs e)
        {
            //sensing.MouseDown = false;
            sensing.MouseDown = false;
        }

        private void mouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;

            //sensing.MouseX = e.X;
            //sensing.MouseY = e.Y;
            //Sensing.Mouse.x = e.X;
            //Sensing.Mouse.y = e.Y;
            sensing.Mouse.X = e.X;
            sensing.Mouse.Y = e.Y;

        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            sensing.Key = e.KeyCode.ToString();
            sensing.KeyPressedTest = true;
        }

        private void keyUp(object sender, KeyEventArgs e)
        {
            sensing.Key = "";
            sensing.KeyPressedTest = false;
        }

        private void Update(object sender, EventArgs e)
        {
            if (sensing.KeyPressed(Keys.Escape))
            {
                START = false;
            }

            if (START)
            {
                this.Refresh();
            }
        }

        #endregion
        /* ------------------- */
        #region Start of Game Methods

        //my
        #region my

        //private void StartScriptAndWait(Func<int> scriptName)
        //{
        //    Task t = Task.Factory.StartNew(scriptName);
        //    t.Wait();
        //}

        //private void StartScript(Func<int> scriptName)
        //{
        //    Task t;
        //    t = Task.Factory.StartNew(scriptName);
        //}

        private int AnimateBackground(int intervalMS)
        {
            while (START)
            {
                setBackgroundPicture(backgroundImages[backgroundImageIndex]);
                Game.WaitMS(intervalMS);
                backgroundImageIndex++;
                if (backgroundImageIndex == 3)
                    backgroundImageIndex = 0;
            }
            return 0;
        }

        private void KlikNaZastavicu()
        {
            foreach (Func<int> f in GreenFlagScripts)
            {
                Task.Factory.StartNew(f);
            }
        }

        #endregion

        /// <summary>
        /// BGL
        /// </summary>
        public BGL()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Pričekaj (pauza) u sekundama.
        /// </summary>
        /// <example>Pričekaj pola sekunde: <code>Wait(0.5);</code></example>
        /// <param name="sekunde">Realan broj.</param>
        public void Wait(double sekunde)
        {
            int ms = (int)(sekunde * 1000);
            Thread.Sleep(ms);
        }

        //private int SlucajanBroj(int min, int max)
        //{
        //    Random r = new Random();
        //    int br = r.Next(min, max + 1);
        //    return br;
        //}

        /// <summary>
        /// -
        /// </summary>
        public void Init()
        {           
            /* Pomocu ove metode se vise ne ucitava SetupGame(), vec se ucitava pocetni zaslon, a 
               SetupGame() se sad poziva s pritiskom buttona */
            if (dt == null) time = dt.TimeOfDay.ToString();
            loopcount++;
            //Load resources and level here
            this.Paint += new PaintEventHandler(DrawTextOnScreen);
            this.setBackgroundPicture("backgrounds\\mountains.jpg");
            setPictureLayout("stretch");
                                 
        }

        /// <summary>
        /// -
        /// </summary>
        /// <param name="val">-</param>
        public void showSyncRate(bool val)
        {
            showSync = val;
            if (val == true) syncRate.Show();
            if (val == false) syncRate.Hide();
        }

        /// <summary>
        /// -
        /// </summary>
        public void updateSyncRate()
        {
            if (showSync == true)
            {
                thisTime = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
                diff = thisTime - lastTime;
                lastTime = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;

                double fr = (1000 / diff) / 1000;

                int fr2 = Convert.ToInt32(fr);

                syncRate.Text = fr2.ToString();
            }

        }

        //stage
        #region Stage

        /// <summary>
        /// Postavi naslov pozornice.
        /// </summary>
        /// <param name="title">tekst koji će se ispisati na vrhu (naslovnoj traci).</param>
        public void SetStageTitle(string title)
        {
            this.Text = title;
        }

        /// <summary>
        /// Postavi boju pozadine.
        /// </summary>
        /// <param name="r">r</param>
        /// <param name="g">g</param>
        /// <param name="b">b</param>
        public void setBackgroundColor(int r, int g, int b)
        {
            this.BackColor = Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Postavi boju pozornice. <c>Color</c> je ugrađeni tip.
        /// </summary>
        /// <param name="color"></param>
        public void setBackgroundColor(Color color)
        {
            this.BackColor = color;
        }

        /// <summary>
        /// Postavi sliku pozornice.
        /// </summary>
        /// <param name="backgroundImage">Naziv (putanja) slike.</param>
        public void setBackgroundPicture(string backgroundImage)
        {
            this.BackgroundImage = new Bitmap(backgroundImage);
        }

        /// <summary>
        /// Izgled slike.
        /// </summary>
        /// <param name="layout">none, tile, stretch, center, zoom</param>
        public void setPictureLayout(string layout)
        {
            if (layout.ToLower() == "none") this.BackgroundImageLayout = ImageLayout.None;
            if (layout.ToLower() == "tile") this.BackgroundImageLayout = ImageLayout.Tile;
            if (layout.ToLower() == "stretch") this.BackgroundImageLayout = ImageLayout.Stretch;
            if (layout.ToLower() == "center") this.BackgroundImageLayout = ImageLayout.Center;
            if (layout.ToLower() == "zoom") this.BackgroundImageLayout = ImageLayout.Zoom;
        }

        #endregion

        //sound
        #region sound methods

        /// <summary>
        /// Učitaj zvuk.
        /// </summary>
        /// <param name="soundNum">-</param>
        /// <param name="file">-</param>
        public void loadSound(int soundNum, string file)
        {
            soundCount++;
            sounds[soundNum] = new SoundPlayer(file);
        }

        /// <summary>
        /// Sviraj zvuk.
        /// </summary>
        /// <param name="soundNum">-</param>
        public void playSound(int soundNum)
        {
            sounds[soundNum].Play();
        }

        /// <summary>
        /// loopSound
        /// </summary>
        /// <param name="soundNum">-</param>
        public void loopSound(int soundNum)
        {
            sounds[soundNum].PlayLooping();
        }

        /// <summary>
        /// Zaustavi zvuk.
        /// </summary>
        /// <param name="soundNum">broj</param>
        public void stopSound(int soundNum)
        {
            sounds[soundNum].Stop();
        }

        #endregion

        //file
        #region file methods

        /// <summary>
        /// Otvori datoteku za čitanje.
        /// </summary>
        /// <param name="fileName">naziv datoteke</param>
        /// <param name="fileNum">broj</param>
        public void openFileToRead(string fileName, int fileNum)
        {
            readFiles[fileNum] = new StreamReader(fileName);
        }

        /// <summary>
        /// Zatvori datoteku.
        /// </summary>
        /// <param name="fileNum">broj</param>
        public void closeFileToRead(int fileNum)
        {
            readFiles[fileNum].Close();
        }

        /// <summary>
        /// Otvori datoteku za pisanje.
        /// </summary>
        /// <param name="fileName">naziv datoteke</param>
        /// <param name="fileNum">broj</param>
        public void openFileToWrite(string fileName, int fileNum)
        {
            writeFiles[fileNum] = new StreamWriter(fileName);
        }

        /// <summary>
        /// Zatvori datoteku.
        /// </summary>
        /// <param name="fileNum">broj</param>
        public void closeFileToWrite(int fileNum)
        {
            writeFiles[fileNum].Close();
        }

        /// <summary>
        /// Zapiši liniju u datoteku.
        /// </summary>
        /// <param name="fileNum">broj datoteke</param>
        /// <param name="line">linija</param>
        public void writeLine(int fileNum, string line)
        {
            writeFiles[fileNum].WriteLine(line);
        }

        /// <summary>
        /// Pročitaj liniju iz datoteke.
        /// </summary>
        /// <param name="fileNum">broj datoteke</param>
        /// <returns>vraća pročitanu liniju</returns>
        public string readLine(int fileNum)
        {
            return readFiles[fileNum].ReadLine();
        }

        /// <summary>
        /// Čita sadržaj datoteke.
        /// </summary>
        /// <param name="fileNum">broj datoteke</param>
        /// <returns>vraća sadržaj</returns>
        public string readFile(int fileNum)
        {
            return readFiles[fileNum].ReadToEnd();
        }

        #endregion

        //mouse & keys
        #region mouse methods

        /// <summary>
        /// Sakrij strelicu miša.
        /// </summary>
        public void hideMouse()
        {
            Cursor.Hide();
        }

        /// <summary>
        /// Pokaži strelicu miša.
        /// </summary>
        public void showMouse()
        {
            Cursor.Show();
        }

        /// <summary>
        /// Provjerava je li miš pritisnut.
        /// </summary>
        /// <returns>true/false</returns>
        public bool isMousePressed()
        {
            //return sensing.MouseDown;
            return sensing.MouseDown;
        }

        /// <summary>
        /// Provjerava je li tipka pritisnuta.
        /// </summary>
        /// <param name="key">naziv tipke</param>
        /// <returns></returns>
        public bool isKeyPressed(string key)
        {
            if (sensing.Key == key)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Provjerava je li tipka pritisnuta.
        /// </summary>
        /// <param name="key">tipka</param>
        /// <returns>true/false</returns>
        public bool isKeyPressed(Keys key)
        {
            if (sensing.Key == key.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion
        /* ------------------- */

        /* ------------ GAME CODE START ------------ */

        /* Game variables */

        /* Initialization 
         
             > Deklarirana su tri objekta pomocu njihovih klasa (vidi klasu Sprite)
             > Kreirana su 2 dogadjaja - pojava zamke (TrapHandler) i obavijest o gubitku (GameOverHandler)
             > Definirana su tri button dogadjaja - pocetak igre, upute, kraj
             
         */

        
        Zeko z;
        Mrkva m;
        Zamka d;

        public delegate void TrapHandler();
        public static event TrapHandler _zamka;

        public delegate void GameOverHandler();
        public static event GameOverHandler _igraGotova;

        /* 
         
           Klikom na Start sve se s pocetnog zaslona skriva, forma se dovodi u prvi plan da se igra moze
           pokrenuti, te se poziva metoda SetupGame() koja inicijalizira objekte i pokrece kreirane skripte.
           
         */
        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Hide();
            btnKraj.Hide();
            btnUpute.Hide();

            lblTitle.Hide();
            Application.OpenForms[this.Name].Focus();
            
            START = true;
            SetupGame();
        }

        /* 
         
           Klikom na Upute korisnik doznaje o igrici i kako pokretati glavnog lika.
           
         */
        private void btnUpute_Click(object sender, EventArgs e)
        {
            UputeBox ub = new UputeBox();
            ub.ShowDialog();
        }

        /* 
         
           Klikom na Kraj korisnika se prvo pita zeli li zaista izaci, a ako je odgovor da, iz igrice se izlazi.
           
         */

        private void btnKraj_Click(object sender, EventArgs e)
        {
            DialogResult dlg = MessageBox.Show("Jeste li sigurni da zelite izaci?", "Igra", MessageBoxButtons.YesNo);
            if (dlg == DialogResult.Yes)
                Application.Exit();
        }


        private void SetupGame()
        {          
            /*
             
             Ovdje se vrsi inicijalizacija igre, postavljanje pozornice i pokretanje skripti, te je takodjer
             zaduzena za pocetni ispis string varijabli navedenih gore. START varijabla potrebna za pokretanje
             cijele igre ovdje se odmah postavlja u true, te se vrsi povezivanje eventova (delegata) s njihovim
             skriptama.
                        
             */

            SetStageTitle("Super Zeko");
            setBackgroundPicture("backgrounds\\mountains.jpg");
            setPictureLayout("stretch");

      
            z = new Zeko("sprites\\zeko.png", GameOptions.RightEdge - 100, 350);
            z.AddCostumes("sprites\\zeko_flip.png");
            z.CurrentCostume = z.Costumes[1];
            Game.AddSprite(z);

            m = new Mrkva("sprites\\mrkva.png", 0, 300);
            m.AddCostumes("sprites\\splot.png");           
            m.SetSize(40);
            Game.AddSprite(m);

            _igraGotova += IgraGotovo;
            _zamka += ZamkaEvent;
          
            START = true;
            Game.StartScript(KretanjeZec);
            Game.StartScript(PojavaMrkve);

            ISPIS = "Uhvaceno: " + z.Bodovi + "\n";
            RAZINA = "Razina: " + z.Razina;           
        }


        /* Event handlers - metode*/

        /*
         
         Ovdje su postavljene metode koje se povezuju s eventovima - Zamka i kretanje/pojava zamke te metoda
         koja zaustavlja igricu i metoda koja raskida vezu izmedju eventova i metoda, kako se ne bi dogodilo
         vise instanci jednog dogadjaja istovremeno.

         ZamkaEvent() - inicijalizira objekt klase Zamka, dodaje ga u igricu i pokrece PojavljivanjeZamke()

         PojavljivanjeZamke() - postavlja zamku na nasumicnu poziciju svakih 0.8 sekundi - ovo se ne mijenja,
         tako da igrac ima moment vremena reagirati i maknuti lika prije nego se okine event kad je igra gotova.
         Ako lik dodiriva zamku pokrece se event _igraGotova.

         IgraGotovo() - brisu se objekti iz memorije, START varijabla se postavlja na false, te se korisnika pita
         zeli li nastaviti. Ukoliko je potvrdan odgovor, igra se ponovno pokrece, a u suprotnom igra zavrsava i izlazi
         se iz aplikacije.

         Dispose() - brisu se poveznice delegata/eventova sa metodama kako ne bi doslo do istovremenog pokretanja
         vise instanci istog eventa.

         */
        private void ZamkaEvent()
        {
            d = new Zamka("sprites\\trap.png", 200, GameOptions.DownEdge - 55);
            Game.AddSprite(d);
            Game.StartScript(PojavljivanjeZamke);
        }

        private int PojavljivanjeZamke()
        {
            Random x = new Random();
            while (START)
            {                
                d.GotoXY(x.Next(GameOptions.LeftEdge + (d.Width + 20), GameOptions.RightEdge - (d.Width + 20)), GameOptions.DownEdge - 55);
                Wait(d.Vrijeme);

                if (d.TouchingSprite(z))
                {
                    _igraGotova.Invoke();
                    break;
                }               
            }

            return 0;
        }

        
        private void IgraGotovo()
        {
            allSprites.Remove(z);
            allSprites.Remove(m);
            allSprites.Remove(d);

            START = false;
            DialogResult dlg = MessageBox.Show("Zelite li ponoviti igru?", "Gotovo!", MessageBoxButtons.YesNo);

            if (dlg == DialogResult.Yes)
            {
                Dispose();
                Wait(1.0);
                SetupGame();
            }

            else
            {
                Application.Exit();
            }
        }


        private void Dispose()
        {
            _zamka = null;
            _igraGotova = null;
        }

        /* Scripts */

        /*
            
            Ovdje se nalaze osnovne skripte za pokretanje 2 preostala lika.

            PojavaMrkve() - mrkva se pojavljuje na nasumicnoj poziciji, te se prikazuje kad se igra pokrene.
            Pocinje s lijeve strane ekrana i krece se prema desnoj strani svojom brzinom po y-osi i nasumicnom 
            putanjom po x-osi. Ako zec uhvati mrkvu, to se pribraja njegovim bodovima, a ako je broj bodova 
            veci od 5, okida se dogadjaj zamke. Ako mrkva dodiriva rub, mijenja se animacija, zaustavlja skripta
            te nakon kratke stanke pokrece opet.
            Brzina mrkve se mijenja ovisno o vremenu, dok je brzina definirana u klasi samo padanje po y-osi. Ako
            se dogodi negativna vrijednost vremena, vrijeme se automatski postavlja na 0.02, inace se smanjuje za
            0.0012.

            KretanjeZec() - zec se krece skacuci od desnog do lijevog ruba ekrana. Zato je bilo potrebno zapamtiti
            njegovu prijasnju poziciju na y-osi (ona s kojom je inicijaliziran) kako bi se zec mogao vratiti na 
            pocetnu visinu. Varijabla brojac zaustavlja zeca da ode previse u visinu. Igrac drzi tipke A i D kako
            bi neprestano skakao i hvatao mrkvice, tako da nema te varijable, zec bi otisao sve do gornjeg ruba
            ekrana. 
            Dakle, dok god je pritisnuta ili tipka A ili tipka D zec se krece u odredjenom smjeru te shodno s tome
            se okrece, ovisno o smjeru u kojem je trenutno. Prvo se podize na odredjenu visinu, a ukoliko korisnik
            otpusti tipku A (ili D) ili brojac dosegne odredjenu vrijednost, zec se vraca na pocetnu Y poziciju dok
            X pozicija ostaje nepromijenjena. Postoji kratka pauza da zec odmah ne napravi drugi skok.
                       
         */

        private int PojavaMrkve()
        {
            Random x = new Random();
            Random y = new Random();

            int x_d = 30;
            int x_g = 35;
            
            m.GotoXY(7, y.Next(0,250));
           
            while (START)
            {               
                m.Show = true;                               
                m.Y += m.Brzina;
                m.X += x.Next(x_d,x_g);
                Wait(m.Vrijeme);

                if (m.TouchingSprite(z))
                {
                    z.Bodovi += m.Vrijednost;
                    if (z.Bodovi == 5)
                        _zamka.Invoke();
                    
                    if (z.Bodovi % 5 == 0 && z.Bodovi != 0)
                    {
                        try
                        {
                            m.Vrijeme -= 0.0012;
                        }

                        catch (VrijemeException v)
                        {
                            m.Vrijeme = 0.02;
                        }
                     
                    }
                     
                    ISPIS = "Uhvaceno: " + z.Bodovi;
                    RAZINA = "Razina: " + z.Razina;
                    m.Show = false;                   
                    Wait(0.3);
                    Game.StartScript(PojavaMrkve);
                    break;
                }

                if (m.TouchingEdge())
                {
                    m.CurrentCostume = m.Costumes[1];
                    Wait(0.3);
                    Game.StartScript(PojavaMrkve);
                    m.CurrentCostume = m.Costumes[0];
                    break;
                }                
            }
            return 0;
        }

        
        private int KretanjeZec()
        {           
            int prijasnjaY = 350;           
            int brojac = 0;

            while (START)
            {
                
                while (sensing.KeyPressed("D"))
                {
                    z.CurrentCostume = z.Costumes[0];                 
                    brojac += 1;
                    z.SetDirection(90);
                    z.Y -= 20;
                    z.MoveSteps(z.Brzina);
                    Wait(0.01);
                    
                    if (sensing.KeyPressed() == false || brojac == 6)
                    {
                        z.Y = prijasnjaY;
                        brojac = 0;
                        Wait(0.3);
                    }
                }


                while (sensing.KeyPressed("A"))
                {
                    z.CurrentCostume = z.Costumes[1];
                    brojac += 1;
                    z.SetDirection(270);
                    z.Y -= 20;
                    z.MoveSteps(z.Brzina);
                    Wait(0.01);
                    
                    if (sensing.KeyPressed() == false || brojac == 7)
                    {
                        z.Y = prijasnjaY;                       
                        brojac = 0;
                        Wait(0.3);
                    }
                                            
                }
             
            }
            return 0;
        }

        

        /* ------------ GAME CODE END ------------ */
    }
}
