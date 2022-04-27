using System;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.DevIl;
using System.Drawing;
using System.Drawing.Imaging;

namespace courswork
{
    public partial class Form1 : Form
    {
        double sizeX = 1, sizeY = 1, sizeZ = 1;
        double a, b, c = 10;
        double deltaY = 10;
        double translateX, translateY, translateZ;
        double VzrivX;
        double podimSamolY = 3;
        double povorotSamX = 0.5;
        double podimSamolZ = 0.5;
        int diriJabaY, samoletY, samoletYY;
        double samoletZ, samoletZZ;
        double cameraSpeed = 5;
        private Explosion explosion = new Explosion(1, 10, 1, 300, 900);
        bool dirIsLive = true;
        bool isFly = false;
        bool isExplosion = false;
        bool letFlt = false;
        int imageId, imageId2, imageId3, imageId4, imageId5;
        uint mGlTextureObject, mGlTextureObject2, mGlTextureObject3, mGlTextureObject4, mGlTextureObject5, DrawTree1;       
        double angle = 10, angleX = -82, angleY = 0, angleZ = -60;
        public Graphics g; //Графика
        public Bitmap derevo; //Битмап
        private double[] _lightFromPos = { 90, -25, 100 }; // позиция из которой испускается свет
        private double[] _lightToPos = { 0, 0, 0 }; // в какую позиция светит источник света
        private uint _shadowMap; // карта теней
        int _shadowMapSize = 512;

        float[] mv = new float[16]; // матрица вида при создании карты теней
        float[] pr = new float[16]; // матрица проекции при создании карты теней

        private void button1_Click(object sender, EventArgs e)
        {
            isFly = true;
            button1.Visible = false;
        }

        public Pen p; //Ручка
        public double angle1 = -Math.PI / 2; //Угол поворота на 90 градусов
        public double ang1 = Math.PI / 4;  //Угол поворота на 45 градусов
        public double ang2 = Math.PI / 6;  //Угол поворота на 30 градусов     

        float global_time = 0;
        bool flagExplosion = false;



        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Gl.glLoadIdentity();

            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDepthFunc(Gl.GL_LESS);

            Gl.glHint(Gl.GL_POLYGON_SMOOTH_HINT, Gl.GL_NICEST);
            Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);

            Gl.glGenTextures(1, out _shadowMap);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, _shadowMap);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_CLAMP_TO_EDGE);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_CLAMP_TO_EDGE);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_COMPARE_MODE_ARB, Gl.GL_COMPARE_R_TO_TEXTURE_ARB);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_COMPARE_FUNC_ARB, Gl.GL_LEQUAL);

            Gl.glTexGeni(Gl.GL_S, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_EYE_LINEAR);
            Gl.glTexGeni(Gl.GL_T, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_EYE_LINEAR);
            Gl.glTexGeni(Gl.GL_R, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_EYE_LINEAR);
            Gl.glTexGeni(Gl.GL_Q, Gl.GL_TEXTURE_GEN_MODE, Gl.GL_EYE_LINEAR);

            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_DEPTH_COMPONENT, _shadowMapSize, _shadowMapSize, 0,
                Gl.GL_DEPTH_COMPONENT, Gl.GL_UNSIGNED_BYTE, null);


            comboBox1.SelectedIndex = 1;
            // инициализация OpenGL
            // инициализация бибилиотеки glut
            Glut.glutInit();
            // инициализация режима экрана
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE);

            Il.ilInit();
            Il.ilEnable(Il.IL_ORIGIN_SET);

            Gl.glClearColor(128, 128, 128, 1);

            // установка порта вывода
            Gl.glViewport(0, 0, AnT.Width, AnT.Height);

            // активация проекционной матрицы
            Gl.glMatrixMode(Gl.GL_PROJECTION);


            Glu.gluPerspective(60, (float)AnT.Width / (float)AnT.Height, 0.1, 800);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);

            int _width = 600;
            int _height = 600;
            derevo = new Bitmap(_width, _height);
            g = Graphics.FromImage(derevo); //Подключаем графику
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//Включаем сглаживание
            p = new Pen(Color.Green);   //Зеленая ручка

            //Вызов рекурсивной функции отрисовки дерева
            DrawTree(300, 0, 200, angle1);

            //Переносим картинку из битмапа на picturebox	
            AnT.BackgroundImage = derevo;

            Gl.glGenTextures(1, out DrawTree1);

            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, DrawTree1);

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE);


            BitmapData data = derevo.LockBits(new System.Drawing.Rectangle(0, 0, _width, _height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, _width, _height, 0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, data.Scan0);

            Gl.glEnable(Gl.GL_DEPTH_TEST);

            Il.ilDeleteImages(1, ref imageId);
            Il.ilGenImages(1, out imageId2);
            Il.ilBindImage(imageId2);
            if (Il.ilLoadImage("../../texturku/korz3.jpg"))
            {
                int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
                int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);
                switch (bitspp)
                {
                    case 24:
                        mGlTextureObject = MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height);
                        break;
                    case 32:
                        mGlTextureObject = MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height);
                        break;
                }
            }
            Il.ilDeleteImages(2, ref imageId2);

            Il.ilDeleteImages(1, ref imageId);
            Il.ilGenImages(1, out imageId2);
            Il.ilBindImage(imageId2);
            if (Il.ilLoadImage("../../texturku/trava.jpg"))
            {
                int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
                int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);
                switch (bitspp)
                {
                    case 24:
                        mGlTextureObject2 = MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height);
                        break;
                    case 32:
                        mGlTextureObject2 = MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height);
                        break;
                }
            }
            Il.ilDeleteImages(2, ref imageId2);
            Il.ilGenImages(1, out imageId3);
            Il.ilBindImage(imageId3);
            if (Il.ilLoadImage("../../texturku/stena.png"))
            {
                int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
                int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);
                switch (bitspp)
                {
                    case 24:
                        mGlTextureObject3 = MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height);
                        break;
                    case 32:
                        mGlTextureObject3 = MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height);
                        break;
                }
            }
            Il.ilDeleteImages(2, ref imageId3);           
            Il.ilDeleteImages(2, ref imageId4);
            Il.ilGenImages(1, out imageId5);
            Il.ilBindImage(imageId5);
            if (Il.ilLoadImage("../../texturku/polosa.jpg"))
            {
                int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
                int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);
                switch (bitspp)
                {
                    case 24:
                        mGlTextureObject5 = MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height);
                        break;
                    case 32:
                        mGlTextureObject5 = MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height);
                        break;
                }
            }
            Il.ilDeleteImages(2, ref imageId5);

            RenderTimer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            global_time += (float)RenderTimer.Interval / 1000;
            Display();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    angle = 10; angleX = -72; angleY = 0; angleZ = -150;
                    translateX = -170; translateY = -200; translateZ = -70;
                    break;
                case 1:
                    angle = 10; angleX = -80; angleY = 0; angleZ = 0;
                    translateX = -13.8f; translateY = 56; translateZ = -35;
                    break;
                case 2:
                    angle = 10; angleX = -92; angleY = 0; angleZ = -45;
                    translateX = -200; translateY = 190; translateZ = -40;
                    break;
            }
            AnT.Focus();
        }


        private void AnT_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.D)
            {
                translateY -= cameraSpeed;

            }
            if (e.KeyCode == Keys.A)
            {
                translateY += cameraSpeed;
               
            }
            if (e.KeyCode == Keys.W)
            {
                translateX += cameraSpeed;
                
            }
            if (e.KeyCode == Keys.S)
            {
                translateX -= cameraSpeed;                

            }
            if (e.KeyCode == Keys.ControlKey)
            {
                translateZ += cameraSpeed;

            }
            if (e.KeyCode == Keys.Space)
            {
                translateZ -= cameraSpeed;
                
            }


            if (e.KeyCode == Keys.Q)
            {
                angleZ -= angle;
            }
            if (e.KeyCode == Keys.E)
            {
                angleZ += angle;
            }
            if (e.KeyCode == Keys.N)
            {
                angleX -= angle;
            }
            if (e.KeyCode == Keys.B)
            {
                angleX += angle;
            }
            if (e.KeyCode == Keys.Z)
            {
                sizeX += 0.1;
            }
            if (e.KeyCode == Keys.X)
            {
                sizeX -= 0.1;
            }

            if (e.KeyCode == Keys.L && diriJabaY < 215)
            {
                if (dirIsLive == true)
                {
                    diriJabaY += 2;
                }
            }

            if (e.KeyCode == Keys.K && diriJabaY > -100)
            {
                if (dirIsLive == true)
                {
                    diriJabaY -= 2;                  
                }
                else {
                }
            }

            if (e.KeyCode == Keys.F1)
            {
                if (isFly == true && letFlt == false)
                {
                    samoletY += 2;
                    c += podimSamolY;
                    if (samoletY >= 90 && letFlt == false && povorotSamX < 88)
                    {
                        povorotSamX += 2.5;

                        if (povorotSamX >= 88)
                        {
                            letFlt = true;
                        }
                    }
                }
                else
                {                   
                    samoletYY += 2;
                    samoletZZ += 0.5;
                }
            }

            if (e.KeyCode == Keys.F2)
            {
                if (isFly == true && samoletY > -52 && samoletZZ == 0)
                {
                    samoletY -= 2;                  
                }
            }
        }


        private void RenderToShadowMap() // создаем карту теней
        {
            Gl.glColorMask(Gl.GL_FALSE, Gl.GL_FALSE, Gl.GL_FALSE, Gl.GL_FALSE);
            Gl.glDisable(Gl.GL_TEXTURE_2D);

            Gl.glEnable(Gl.GL_POLYGON_OFFSET_FILL);
            Gl.glPolygonOffset(4, 4);

            // устанавливаем проекцию истоника света
            Gl.glViewport(0, 0, _shadowMapSize, _shadowMapSize);
            Gl.glClear(Gl.GL_DEPTH_BUFFER_BIT);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            Glu.gluPerspective(140, 1, 0.1, 1500);
            Glu.gluLookAt(_lightFromPos[0], _lightFromPos[1], _lightFromPos[2],        // eye
            _lightToPos[0], _lightToPos[1], _lightToPos[2], // center
                0, 1, 0);                     // up

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            // получаем матрицы вида и проекции
            Gl.glGetFloatv(Gl.GL_MODELVIEW_MATRIX, mv);
            Gl.glGetFloatv(Gl.GL_PROJECTION_MATRIX, pr);

            // отрисовываем сцену с точки зрения источника света
            Draw();

            // копируем текстуру глубины в карту теней
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, _shadowMap);
            Gl.glCopyTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_DEPTH_COMPONENT, 0, 0, _shadowMapSize, _shadowMapSize, 0);

            // возвращаем состояние для обычного отображения
            Gl.glDisable(Gl.GL_POLYGON_OFFSET_FILL);
            Gl.glColorMask(Gl.GL_TRUE, Gl.GL_TRUE, Gl.GL_TRUE, Gl.GL_TRUE);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
        }


        private void Display()
        {
            RenderToShadowMap(); // отрисовываем карту теней

            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            // устанавливаем положение главной камеры
            Gl.glViewport(0, 0, AnT.Width, AnT.Height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            Glu.gluPerspective(45, AnT.Width / (float)AnT.Height, 0.1, 800);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);

            Gl.glClearColor(255, 255, 255, 1);
        

            Gl.glPushMatrix();

            Gl.glRotated(angleX, 1, 0, 0); Gl.glRotated(angleY, 0, 1, 0); Gl.glRotated(angleZ, 0, 0, 1);

            if (comboBox1.SelectedIndex == 1)
            {
                Gl.glTranslated(translateX, translateY - diriJabaY, translateZ);
            }
            else
            {
                Gl.glTranslated(translateX, translateY, translateZ);
            }

            // устанавлвиаем карту теней
            Gl.glActiveTextureARB(Gl.GL_TEXTURE1_ARB);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, _shadowMap);

            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_TEXTURE_GEN_S);
            Gl.glEnable(Gl.GL_TEXTURE_GEN_T);
            Gl.glEnable(Gl.GL_TEXTURE_GEN_R);
            Gl.glEnable(Gl.GL_TEXTURE_GEN_Q);

            Gl.glTexGenfv(Gl.GL_S, Gl.GL_EYE_PLANE, new[] { 1f, 0f, 0f, 0f });
            Gl.glTexGenfv(Gl.GL_T, Gl.GL_EYE_PLANE, new[] { 0f, 1f, 0f, 0f });
            Gl.glTexGenfv(Gl.GL_R, Gl.GL_EYE_PLANE, new[] { 0f, 0f, 1f, 0f });
            Gl.glTexGenfv(Gl.GL_Q, Gl.GL_EYE_PLANE, new[] { 0f, 0f, 0f, 1f });

            Gl.glTexEnvi(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE);


            // корректируем текстурные координаты
            Gl.glMatrixMode(Gl.GL_TEXTURE);
            Gl.glPushMatrix();

            Gl.glLoadIdentity();
            Gl.glTranslatef(0.5f, 0.5f, 0.5f);     // remap from [-1,1]^2 to [0,1]^2
            Gl.glScalef(0.5f, 0.5f, 0.5f);
            Gl.glMultMatrixf(pr);
            Gl.glMultMatrixf(mv);

            Gl.glActiveTextureARB(Gl.GL_TEXTURE0_ARB);

            Draw(); // отрисовываем сцену

            Gl.glActiveTextureARB(Gl.GL_TEXTURE1_ARB);
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glActiveTextureARB(Gl.GL_TEXTURE0_ARB);

            // рисуем источники света
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glPopMatrix();
            Gl.glPushMatrix();

            Gl.glTranslatef((float)_lightFromPos[0], (float)_lightFromPos[1], (float)_lightFromPos[2]);
            Gl.glActiveTextureARB(Gl.GL_TEXTURE0_ARB);
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Glut.glutSolidSphere(0.1f, 15, 15);
            Gl.glPopMatrix();

            Gl.glMatrixMode(Gl.GL_TEXTURE);
            Gl.glPopMatrix();
         
            Gl.glMatrixMode(Gl.GL_MODELVIEW);

            Gl.glFlush();

            AnT.Invalidate();
        }

        private void Draw()
        {          
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            explosion.Calculate(global_time);
            Gl.glPushMatrix();
            if (isExplosion && !flagExplosion)
            {
                flagExplosion = true;
                explosion.SetNewPosition(24, 100, 45);
                explosion.SetNewPower(600);
                explosion.Boooom(global_time, 100, 300);
                comboBox1.SelectedIndex = 0;
            }
           

            /////////////////////
            // ВЕТРЯК
            /////////////////////

            Gl.glPushMatrix();
            Gl.glColor3f(0.75f, 0.75f, 0.75f);
            Gl.glTranslated(15, 90.5f, 0);
            Glut.glutSolidCylinder(2, 46, 15, 1);

            Gl.glTranslated(0, 0, 43);
            Gl.glRotated(90, 0, 1, 0);
            Glut.glutSolidCylinder(2, 5, 15, 1);

            Gl.glPopMatrix();

            /////////////////////
            // ЛОПОСТИ ВЕТРЯКА
            /////////////////////
            Gl.glPushMatrix();
            Gl.glTranslated(20.1f, 90.7f, 43);


            Gl.glColor3f(1, 1, 1);            
            Gl.glRotated(90, 0, 0, 1);
            Gl.glRotated(a, 0, 1, 0);
            Gl.glScaled(0.5f, 0.1f, 1);
            Glut.glutSolidCylinder(2, 25, 15, 1);

            a += deltaY;           

            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(20.1f, 90.7f, 43);

 
            Gl.glColor3f(0, 0, 1);
            Gl.glRotated(90, 0, 0, 1);
            Gl.glRotated(a+120, 0, 1, 0);
            Gl.glScaled(0.5f, 0.1f, 1);
            Glut.glutSolidCylinder(2, 25, 15, 1);

            Gl.glPopMatrix();
            Gl.glPushMatrix();
            Gl.glTranslated(20.1f, 90.7f, 43);

            Gl.glColor3f(1, 0, 0);
            Gl.glRotated(90, 0, 0, 1);
            Gl.glRotated(a + 220, 0, 1, 0);
            Gl.glScaled(0.5f, 0.1f, 1);
            Glut.glutSolidCylinder(2, 25, 15, 1);

            Gl.glPopMatrix();

            /////////////////////
            // ДИРИЖОБЭЛЬ И ВЗРЫВ
            /////////////////////
            VzrivX = Math.Sqrt(0 + (diriJabaY - 120) * (diriJabaY - 120) + 0);

            if (VzrivX > 10 && isExplosion == false)
            {
                Gl.glPushMatrix();
                Gl.glTranslated(0, diriJabaY, 0);
                Gl.glPushMatrix();
                Gl.glColor3f(1, 0.92f, 0);
                Gl.glScaled(1, 4, -1);               
                Gl.glTranslated(15, -15, -55);
                Glut.glutSolidSphere(10, 12, 15);
                Gl.glColor3f(0.52f, 0.80f, 1);
                Gl.glTranslated(0, 0, 4.5);
                Glut.glutSolidSphere(10, 12, 15);
                Gl.glPopMatrix();

                /////////////////////
                // КОРЗИНА  
                /////////////////////
                ///

                Gl.glPushMatrix();
                Gl.glColor3f(1, 1, 1);
                Gl.glEnable(Gl.GL_TEXTURE_2D);
                Gl.glEnable(Gl.GL_BLEND);
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, mGlTextureObject);
                Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

                Gl.glPushMatrix();
                Gl.glTranslated(10, -46.5f, 25);

                Gl.glBegin(Gl.GL_QUADS);
                Gl.glTexCoord2f(1, 0);
                Gl.glVertex3d(0, 0, 0);
                Gl.glTexCoord2f(0, 0);
                Gl.glVertex3d(10, 0, 0);
                Gl.glTexCoord2f(0, 1);
                Gl.glVertex3d(10, 0, 7);
                Gl.glTexCoord2f(1, 1);
                Gl.glVertex3d(0, 0, 7);
                Gl.glEnd();

                Gl.glBegin(Gl.GL_QUADS);
                Gl.glTexCoord2f(1, 0);
                Gl.glVertex3d(0, -25, 0);
                Gl.glTexCoord2f(0, 0);
                Gl.glVertex3d(10, -25, 0);
                Gl.glTexCoord2f(0, 1);
                Gl.glVertex3d(10, -25, 7);
                Gl.glTexCoord2f(1, 1);
                Gl.glVertex3d(0, -25, 7);
                Gl.glEnd();

                Gl.glBegin(Gl.GL_QUADS);
                Gl.glTexCoord2f(1, 0);
                Gl.glVertex3d(0, 0, 0);
                Gl.glTexCoord2f(0, 0);
                Gl.glVertex3d(0, -25, 0);
                Gl.glTexCoord2f(0, 1);
                Gl.glVertex3d(0, -25, 7);
                Gl.glTexCoord2f(1, 1);
                Gl.glVertex3d(0, 0, 7);
                Gl.glEnd();

                Gl.glBegin(Gl.GL_QUADS);
                Gl.glTexCoord2f(1, 0);
                Gl.glVertex3d(10, 0, 0);
                Gl.glTexCoord2f(0, 0);
                Gl.glVertex3d(10, -25, 0);
                Gl.glTexCoord2f(0, 1);
                Gl.glVertex3d(10, -25, 7);
                Gl.glTexCoord2f(1, 1);
                Gl.glVertex3d(10, 0, 7);
                Gl.glEnd();

                Gl.glBegin(Gl.GL_QUADS);
                Gl.glTexCoord2f(1, 0);
                Gl.glVertex3d(0, 0, 0);
                Gl.glTexCoord2f(0, 0);
                Gl.glVertex3d(10, 0, 0);
                Gl.glTexCoord2f(0, 1);
                Gl.glVertex3d(10, -25, 0);
                Gl.glTexCoord2f(1, 1);
                Gl.glVertex3d(0, -25, 0);
                Gl.glEnd();

                Gl.glDisable(Gl.GL_BLEND);
                Gl.glDisable(Gl.GL_TEXTURE_2D);
                Gl.glPopMatrix();

                /////////////////////
                // ТРОСЫ  
                /////////////////////
                ///

                Gl.glPushMatrix();
                Gl.glColor3f(1, 0.72f, 0);

                Gl.glTranslated(10, -46.5f, 25);
                Glut.glutSolidCylinder(0.1f, 36, 15, 1);

                Gl.glTranslated(0, -25f, 0);
                Glut.glutSolidCylinder(0.1f, 36, 15, 1);

                Gl.glTranslated(10, 0, 0);
                Glut.glutSolidCylinder(0.1f, 36, 15, 1);

                Gl.glTranslated(0, 25, 0);
                Glut.glutSolidCylinder(0.1f, 36, 15, 1);
                Gl.glPopMatrix();

                Gl.glPopMatrix();
                Gl.glPopMatrix();
            }
            else
            {
                isExplosion = true;
                dirIsLive = false;
            }

            /////////////////////
            // САМОЛЁТ В АНГАРЕ
            /////////////////////
            ///            
            Gl.glPushMatrix();
            if (letFlt == true)
            {
                Gl.glTranslated(0, samoletYY, samoletZZ);
            }                     
                Gl.glRotated(270, 0, 0, 1);
                Gl.glTranslated(135, -75 + samoletY, 12.5);
                Gl.glRotated(povorotSamX, 0, 0, 1);

                //КОРПУС
                Gl.glPushMatrix();
                Gl.glColor3f(0.45f, 0.52f, 0.58f);
                Gl.glScaled(0.8, 4, -1);
                Glut.glutSolidSphere(5, 12, 15);
                Gl.glPopMatrix();

                //ЛОПОСТЬ                       
                Gl.glPushMatrix();

                Gl.glColor3f(1, 1, 0.18f);
                Gl.glRotated(c, 0, 1, 0);
                Gl.glScaled(0.2, 0.1, 1);
                Gl.glTranslated(0, 200, 0);
                Glut.glutSolidCube(5);
                Gl.glPopMatrix();

                //КРЫЛЬЯ
                Gl.glPushMatrix();

                Gl.glColor3f(0.18f, 0.83f, 0.78f);
                Gl.glScaled(10, 1.5, 0.1);
                Gl.glTranslated(0, 1.5, 50);
                Glut.glutSolidCube(5);

                Gl.glTranslated(0, 0, -100);
                Glut.glutSolidCube(5);

                Gl.glPopMatrix();

                //ТРОСЫ
                Gl.glPushMatrix();
                Gl.glColor3f(1, 0.72f, 0);
                Gl.glTranslated(1.5, 2.5, -5);
                Gl.glRotated(65, 0, 1, 0);
                Glut.glutSolidCylinder(0.1f, 24, 15, 1);

                Gl.glRotated(-65, 0, 1, 0);
                Gl.glTranslated(-3.5, 0, 0);
                Gl.glRotated(-65, 0, 1, 0);
                Glut.glutSolidCylinder(0.1f, 24, 15, 1);

                Gl.glRotated(65, 0, 1, 0);
                Gl.glTranslated(-20, 0, 0);
                Gl.glRotated(65, 0, 1, 0);
                Glut.glutSolidCylinder(0.1f, 24, 15, 1);

                Gl.glRotated(-65, 0, 1, 0);
                Gl.glTranslated(45, 0, 0);
                Gl.glRotated(-65, 0, 1, 0);
                Glut.glutSolidCylinder(0.1f, 24, 15, 1);
                Gl.glPopMatrix();

                //ПАЛКИ ДЛЯ КОЛЁС
                Gl.glPushMatrix();
                Gl.glScaled(0.1, 0.1, 1);
                Gl.glTranslated(123, 25, -6);
                Glut.glutSolidCylinder(2, 1, 15, 1);

                Gl.glTranslated(-246, 0, 0);
                Glut.glutSolidCylinder(2, 1, 15, 1);

                Gl.glTranslated(123, -130, 0);
                Glut.glutSolidCylinder(2, 4, 15, 1);
                Gl.glPopMatrix();

                //КОЛЁСА ДЛЯ КОЛЁС
                Gl.glPushMatrix();
                Gl.glColor3f(0, 0, 0);
                Gl.glScaled(0.3, 0.5, 0.5);
                Gl.glTranslated(41.05, 5, -16);
                Glut.glutSolidSphere(4, 14, 14);

                Gl.glTranslated(-82.1, 0, 0);
                Glut.glutSolidSphere(4, 14, 14);

                Gl.glTranslated(41.05, -26, 2);
                Glut.glutSolidSphere(4, 14, 14);
                Gl.glPopMatrix();            

            Gl.glPopMatrix();

            /////////////////////
            // САМОЛЁТ, ПРОПЕЛЛЕР, КРЫЛЬЯ, ТРОСЫ, КОЛЁСА
            /////////////////////
            ///
            Gl.glPushMatrix();
            Gl.glRotated(4, 1, 0, 0);
            Gl.glTranslated(80 , -140 + b, 22 + samoletZ);

            //КОРПУС
            Gl.glPushMatrix();                              
            Gl.glColor3f(0.45f, 0.52f, 0.58f);
            Gl.glScaled(0.8, 4, -1);           
            Glut.glutSolidSphere(5, 12, 15);
            Gl.glPopMatrix();

            //ЛОПОСТЬ                       
            Gl.glPushMatrix();
            if (isFly == true)
            {
                if (b > 90 ) {
                    samoletZ += podimSamolZ;
                        }
                b += podimSamolY;
            }

            Gl.glColor3f(1, 1, 0.18f);
            Gl.glRotated(b, 0, 1, 0);
            Gl.glScaled(0.2, 0.1, 1);
            Gl.glTranslated(0, 200, 0);
            Glut.glutSolidCube(5);
            Gl.glPopMatrix();

            

            //КРЫЛЬЯ
            Gl.glPushMatrix();

            Gl.glColor3f(0.18f, 0.83f, 0.78f);
            Gl.glScaled(10, 1.5, 0.1);            
            Gl.glTranslated(0, 1.5, 50);
            Glut.glutSolidCube(5);

            Gl.glTranslated(0, 0, -100);
            Glut.glutSolidCube(5);

            Gl.glPopMatrix();

            //ТРОСЫ
            Gl.glPushMatrix();
            Gl.glColor3f(1, 0.72f, 0);
            Gl.glTranslated(1.5, 2.5, -5);
            Gl.glRotated(65, 0, 1, 0);
            Glut.glutSolidCylinder(0.1f, 24, 15, 1);

            Gl.glRotated(-65, 0, 1, 0);
            Gl.glTranslated(-3.5, 0, 0);
            Gl.glRotated(-65, 0, 1, 0);
            Glut.glutSolidCylinder(0.1f, 24, 15, 1);

            Gl.glRotated(65, 0, 1, 0);
            Gl.glTranslated(-20, 0, 0);
            Gl.glRotated(65, 0, 1, 0);
            Glut.glutSolidCylinder(0.1f, 24, 15, 1);

            Gl.glRotated(-65, 0, 1, 0);
            Gl.glTranslated(45, 0, 0);
            Gl.glRotated(-65, 0, 1, 0);
            Glut.glutSolidCylinder(0.1f, 24, 15, 1);
            Gl.glPopMatrix();

            //ПАЛКИ ДЛЯ КОЛЁС
            Gl.glPushMatrix();
            Gl.glScaled(0.1, 0.1, 1);
            Gl.glTranslated(123, 25, -6);
            Glut.glutSolidCylinder(2, 1, 15, 1);
          
            Gl.glTranslated(-246, 0, 0);
            Glut.glutSolidCylinder(2, 1, 15, 1);

            Gl.glTranslated(123, -130, 0);
            Glut.glutSolidCylinder(2, 4, 15, 1);
            Gl.glPopMatrix();

            //КОЛЁСА ДЛЯ КОЛЁС
            Gl.glPushMatrix();
            Gl.glColor3f(0, 0, 0);
            Gl.glScaled(0.3, 0.5, 0.5);
            Gl.glTranslated(41.05, 5, -16);
            Glut.glutSolidSphere(4, 14, 14);

            Gl.glTranslated(-82.1, 0, 0);
            Glut.glutSolidSphere(4, 14, 14);

            Gl.glTranslated(41.05, -26, 2);
            Glut.glutSolidSphere(4, 14, 14);
            Gl.glPopMatrix();

            Gl.glPopMatrix();

            /////////////////////
            // ВЗЛЁТНЫЕ ПОЛОСЫ 
            /////////////////////
            ///
            Gl.glPushMatrix();
            Gl.glColor3f(1, 1, 1);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, mGlTextureObject5);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);


            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(60, -200, 2);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(100, -200, 2);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3d(100, 190, 2);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(60, 190, 2);
            Gl.glEnd();


            Gl.glDisable(Gl.GL_BLEND);
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glPopMatrix();


            /////////////////////
            // ВЗЛЁТНЫЕ ПОЛОСЫ ИЗ АНГАРА
            /////////////////////
            ///
            Gl.glPushMatrix();
            Gl.glColor3f(1, 1, 1);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, mGlTextureObject5);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);


            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(-0, -155, 2);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(-0, -115, 2);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3d(75, -115, 2);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(75, -155, 2);
            Gl.glEnd();


            Gl.glDisable(Gl.GL_BLEND);
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glPopMatrix();

            /////////////////////
            // ФОН
            /////////////////////
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, DrawTree1);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

            Gl.glPushMatrix();
            Gl.glTranslated(-200, -200, -50);

            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(0, 0, 50);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(400, 0, 50);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3d(400, 0, 250);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(0, 0, 250);
            Gl.glEnd();

            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(0, 0, 50);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(0, 400, 50);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3d(0, 400, 250);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(0, 0, 250);
            Gl.glEnd();


            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(0, 400, 50);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(400, 400, 50);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3d(400, 400, 250);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(0, 400, 250);
            Gl.glEnd();


            Gl.glDisable(Gl.GL_BLEND);
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glPopMatrix();

            /////////////////////
            // ТРАВА ГОВНА 
            /////////////////////
            Gl.glPushMatrix();
            Gl.glColor3f(1, 1, 1);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, mGlTextureObject2);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);


            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(200, 200, 0);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(-200, 200, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(-200, -200, 0);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3d(200, -200, 0);
            Gl.glEnd();


            Gl.glDisable(Gl.GL_BLEND);
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glPopMatrix();

            /////////////////////
            // АНГАР  
            /////////////////////
            Gl.glPushMatrix();
            Gl.glColor3f(1, 1, 1);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, mGlTextureObject);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
                    
            // СТЕНА ПРАВА
            Gl.glPushMatrix();
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(-150, -100, 0);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(0, -100, 0);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3d(0, -100, 40);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(-150, -100, 40);
            Gl.glEnd();

            // СТЕНА ЛЕВО
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(-150, -170, 0);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(0, -170, 0);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3d(0, -170, 40);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(-150, -170, 40);
            Gl.glEnd();

            // ПОЛ
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(-150, -100, 2);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(0, -100, 2);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3d(0, -170, 2);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(-150, -170, 2);
            Gl.glEnd();

            // ЗАДНИК
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(-150, -100, 0);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3d(-150, -100, 40);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(-150, -170, 40);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(-150, -170, 0);
            Gl.glEnd();

            // ЗВДНИК ТРЕУГОЛЬНИК
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(-150, -100, 40);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(-150, -170, 40);
            Gl.glTexCoord2f(0.5f, 1);
            Gl.glVertex3d(-150, -135, 60);                 
            Gl.glEnd();

            // КРЫША ЛЕВО
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(-150, -170, 40);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3d(0, -170, 40);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(0, -135, 60);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(-150, -135, 60);
            Gl.glEnd();

            // КРЫША ПРАВО
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(-150, -100, 40);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3d(0, -100, 40);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(0, -135, 60);
            Gl.glTexCoord2f(1, 0);
            Gl.glVertex3d(-150, -135, 60);
            Gl.glEnd();

            Gl.glPopMatrix();
            Gl.glPopMatrix();


            Gl.glPushMatrix();

            Gl.glPopMatrix();

            //// возвращаем состояние матрицы
            Gl.glPopMatrix();
        }

        public int DrawTree(double x, double y, double a, double angle1)
        {

            if (a > 2)
            {
                a *= 0.7; //Меняем параметр a

                //Считаем координаты для вершины-ребенка
                double xnew = Math.Round(x + a * Math.Cos(angle1)),
                       ynew = Math.Round(y - a * Math.Sin(angle1));

                //рисуем линию между вершинами
                g.DrawLine(p, (float)x, (float)y, (float)xnew, (float)ynew);

                //Переприсваеваем координаты
                x = xnew;
                y = ynew;

                //Вызываем рекурсивную функцию для левого и правого ребенка
                DrawTree(x, y, a, angle1 + ang1);
                DrawTree(x, y, a, angle1 - ang2);
            }
            return 0;
        }

        private static uint MakeGlTexture(int Format, IntPtr pixels, int w, int h)
        {

            // идентификатор текстурного объекта
            uint texObject;

            // генерируем текстурный объект
            Gl.glGenTextures(1, out texObject);

            // устанавливаем режим упаковки пикселей
            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);

            // создаем привязку к только что созданной текстуре
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texObject);

            // устанавливаем режим фильтрации и повторения текстуры
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_MODULATE);

            // создаем RGB или RGBA текстуру
            switch (Format)
            {
                case Gl.GL_RGB:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB, w, h, 0, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, pixels);
                    break;

                case Gl.GL_RGBA:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, w, h, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, pixels);
                    break;
            }

            return texObject;

        }
    }
}