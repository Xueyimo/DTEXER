using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using CsGL.OpenGL;

namespace arcball
{
    public partial class csgl : Component
    {
        public csgl()
        {
            InitializeComponent();
        }

        public csgl(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

    }
    #region OpenGL Control
    public  class OGL : OpenGLControl
    {
        #region variable definitions
        private Point mouseStartDrag;
        private Point mouseEndDrag;
        private static int height = 0;
        private static int width = 0;

        private System.Object matrixLock = new System.Object();
        private arcball arcBall = new arcball(640.0f, 480.0f);
        private float[] matrix = new float[16];
        private Matrix4f LastRot = new Matrix4f();
        private Matrix4f ThisRot = new Matrix4f();
        private float LastScale = 1.0f;

        private static bool isLeftDrag = false;
        private static bool isRightDrag = false;
        private static bool isMiddleDrag = false;

        #region show_special_object
        public PO p_tags;//记录被标记的点
        public PO p_tags_last;//上一个被标记的点
        public LinkList<PO> pl;//显示生成的点链--绿色
        public LinkList<PO> pl_tag;//显示需要特殊标记的点-黄色
        public LinkList<PO> model_special_pl;//显示模型需要特殊标记的点-黄色：如井位、边界控制点
        public LinkList<SURFACE> sl,sl_tag;//sl_tag用于标定要寻找的面
        public LINE l_tag;//将要被标记的边
        public LinkList<LINE> ll;
        private LinkList<LINE> ll_pebi;
        private LinkList<LINE> ll_triangle;
        public LinkList<LINE> ll_tag;//显示需要特殊标记的线-黄色
        public PO[] p2_shadow=new PO[2];//绘制边界拉动时的两条边
        #endregion
        //光源位置 
        float lx = 0.0f;
        float ly = 0.0f;
        float lz = -50.0f;

        float[] fLightPosition = new float[4];// 光源位置 
        float[] fLightAmbient = new float[4] { 1f, 1f, 1f, 1.0f };// 环境光参数 
        float[] fLightDiffuse = new float[4] { 1f, 1f, 1f, 1f };// 漫射光参数

      //  Camera m_Camera = new Camera();
        //Camera
        double angle = 90;//控制观察的范围的大小
        double left_right = 0;//控制摄像机的左右移动
        double top_down = 0;//控制摄像机的上下移动

        //model
        container c;//模型对象
        public LinkList<LINE> model_boundary_ll;//模型边界构成的边链
        public bool ADD_MODEL_vwell=false;//标志是否添加直井
        public bool ADD_MODEL_fault = false;//标识是否添加断层
        public bool ADD_MODEL_hwell = false;//标识是否添加水平井

        //Display control
        public bool SHOW_PEBI=false;//显示PEBI网格
        public bool SHOW_TRIANGLE = false;//显示三角网格
        public bool SHOW_MODEL_BOUNDARY_CONTROL = false;//显示模型的边界及相关控制点
        public bool SHOW_ORDINARY_POINT = false;//显示普通点
        public bool SHOW_ORDINARY_LL= false;//显示普通边
        public bool SHOW_EDIT_MODEL = false;//显示特殊的用于模型编辑的点
        #endregion

        public OGL(): base()
        {
            GC.Collect();
            pl = new LinkList<PO>();
            pl_tag = new LinkList<PO>();
            ll = new LinkList<LINE>();
            ll_triangle=new LinkList<LINE>();
            ll_pebi = new LinkList<LINE>();
            ll_tag = new LinkList<LINE>();
            sl = new LinkList<SURFACE>();
            sl_tag = new LinkList<SURFACE>();
            model_boundary_ll = new LinkList<LINE>();
            model_special_pl = new LinkList<PO>();
        }

        protected override void InitGLContext()
        {
            GL.glShadeModel(GL.GL_SMOOTH);								// enable smooth shading
            GL.glClearColor(1.0f, 1.0f, 1.0f, 0.5f);					// black background
            GL.glClearDepth(1.0f);										// depth buffer setup
            GL.glEnable(GL.GL_DEPTH_TEST);								// enables depth testing
            GL.glDepthFunc(GL.GL_LEQUAL);								// type of depth testing
            fLightPosition = new float[4] { lx, ly, lz, 1f };
            GL.glLightfv(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, fLightPosition);//光源位置
            GL.glEnable(GL.GL_LIGHTING);                                            // Enable Lighting
            GL.glEnable(GL.GL_LIGHT0);                                            // Enable Default Light
            GL.glEnable(GL.GL_COLOR_MATERIAL);   // Enable Color Material
            GL.glLoadIdentity();
            GL.gluPerspective(90,1, 20.0f, 40.0f);
            GL.gluLookAt(0.0f, 0.0f, 10.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
            
            //GL.glLightfv(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, fLightAmbient);//环境光源 
            //GL.glLightfv(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, fLightDiffuse);//漫射光源 
            
            //GL.glEnable(OpenGL.GL_LIGHTING);//开启光照 
            //GL.glEnable(OpenGL.GL_LIGHT0);

            //GL.glEnable(OpenGL.GL_NORMALIZE);

            //GL.glClearColor(0, 0, 0, 0);

            //m_Camera.setCamera(0.0f, 0.0f, 6.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
            //m_Camera.setSpeed(1f);
            //GL.glClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            //GL.gluOrtho2D(-50,50,-50,50);
            #region mouse handles
            MouseControl mouseControl = new MouseControl(this);
            mouseControl.AddControl(this);
            mouseControl.LeftMouseDown += new MouseEventHandler(glOnLeftMouseDown);
            mouseControl.LeftMouseUp += new MouseEventHandler(glOnLeftMouseUp);

            mouseControl.RightMouseDown += new MouseEventHandler(glOnRightMouseDown);
            mouseControl.RightMouseUp += new MouseEventHandler(glOnRightMouseUp);
            mouseControl.MiddleMouseDown += new MouseEventHandler(glOnMiddleMouseDown);
            mouseControl.MiddleMouseUp += new MouseEventHandler(glOnMiddleMouseUp);
            mouseControl.MouseWheel += new MouseEventHandler(glOnMouseWheel);
            #endregion
            this.KeyDown += new KeyEventHandler(glOnKeyDown);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Size s = Size;
            height = s.Height;
            width = s.Width;

            GL.glViewport(0, 0, width, height);

            GL.glPushMatrix();
            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();
            GL.gluPerspective(25.0, (double)width / (double)height, 1.0, 15.0);
            GL.glTranslatef(0.0f, 0.0f, -4.0f);
            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glPopMatrix();
            arcBall.setBounds((float)width, (float)height); //*NEW* Update mouse bounds for arcball
        }
        #region Graphics_model_operation
        public void GetPL(PO pl1)
        {
            pl.Insert(pl1);
            return;
        }
        public void SetPL(LinkList<PO> pl)
        {
            pl_tag = pl;
            return;
        }
        public void GetLL(LINE l0)
        {
            ll.Insert(l0);
            return;
        }
        public void GetLL_pebi(LINE l0)
        {
            ll_pebi.Insert(l0);
            return;
        }
        public void GetLL_triangle(LINE l0)
        {
            ll_triangle.Insert(l0);
            return;
        }
        public void GetSL(SURFACE s0)
        {
            sl.Insert(s0);
            return;
        }
        public bool Equal(PO p1, PO p2)
        {
            if (System.Math.Abs(p1.x - p2.x) < 0.1 && System.Math.Abs(p1.y - p2.y) < 0.1 && System.Math.Abs(p1.z - p2.z) < 0.1)
                return true;
            else
                return false;
        }
        public void Seek_Tag(PO p1, PO p2, PO p3)
        {
            Node<SURFACE> sn = sl.Head;
            while (sn != null)
            {
                if (Equal(sn.Data.p1, p1) && Equal(sn.Data.p2, p2) && Equal(sn.Data.p3, p3))
                    break;
                if (Equal(sn.Data.p1, p1) && Equal(sn.Data.p2, p3) && Equal(sn.Data.p3, p1))
                    break;
                if (Equal(sn.Data.p1, p2) && Equal(sn.Data.p2, p1) && Equal(sn.Data.p3, p3))
                    break;
                if (Equal(sn.Data.p1, p2) && Equal(sn.Data.p2, p3) && Equal(sn.Data.p3, p1))
                    break;
                if (Equal(sn.Data.p1, p3) && Equal(sn.Data.p2, p2) && Equal(sn.Data.p3, p1))
                    break;
                if (Equal(sn.Data.p1, p3) && Equal(sn.Data.p2, p1) && Equal(sn.Data.p3, p2))
                    break;
                sn = sn.Next;
            }
            if (sn != null)
            {
                sl_tag.Insert(sn.Data);
                this.PlotGLKey();
            }
            else
                MessageBox.Show("no Existing!!");
        }
        public void Seek_Tag_LINE(PO p1, PO p2)
        {
            Node<LINE> ln = ll.Head;
            while (ln != null)
            {
                if (Equal(ln.Data.p1, p1) && Equal(ln.Data.p2, p2))
                    break;
                if (Equal(ln.Data.p1, p2) && Equal(ln.Data.p2, p1))
                    break;
                ln = ln.Next;
            }
            if (ln != null)
            {
                l_tag = ln.Data;
                this.PlotGLKey();
            }
            else
                MessageBox.Show("no Existing!!");
        }
        public void Seek_Tag_PO(PO p0)
        {
            Node<PO> pn = pl.Head;
            util u1 = new util();
            PO pt=null;
            while (pn != null)
            {
                if (pn.Data.Me_to_po_length(p0)<1)
                {
                    pt = pn.Data;
                    break;
                }
                pn = pn.Next;
            }
            p_tags = pt;
        }
        public void Seek_Special_PO(PO p0)
        {
            Node<PO> pn =model_special_pl.Head;
            util u1 = new util();
            PO pt = null;
            while (pn != null)
            {
                if (pn.Data.Me_to_po_length(p0) < 1)
                {
                    pt = pn.Data;
                    break;
                }
                pn = pn.Next;
            }
            p_tags = pt;
        }
        public void Set_model(container c0)
        {
            c = c0;
        }
        public void Clear()
        {
            pl.Clear();
            pl_tag.Clear();
            model_boundary_ll.Clear();
            ll.Clear();
            ll_tag.Clear();
            ll_triangle.Clear();
            ll_pebi.Clear();
            p_tags = null;
        }

        #endregion Graphics_model_operation
        public PO Map_to_Space(int x, int y)
        {
            util u1 = new util();
            unsafe
            {
                double* modelview = stackalloc double[16];
                double* projection = stackalloc double[16];
                int* viewport = stackalloc int[4];
                PO pf;
                double z;
                pf = new PO();
                GLU.glGetDoublev(GLU.GL_MODELVIEW_MATRIX, modelview);
                GLU.glGetDoublev(GLU.GL_PROJECTION_MATRIX, projection);
                GLU.glGetIntegerv(GLU.GL_VIEWPORT, viewport);
                GLU.glReadPixels(x, y, 1, 1, GLU.GL_DEPTH_COMPONENT, GLU.GL_FLOAT, &z);
                double world_x, world_y, world_z;
                GLU.gluUnProject((double)x, (double)(viewport[3] - y - 1), 1,
                    modelview, projection, viewport,
                    &world_x, &world_y, &world_z);
                pf.x = (float)world_x;
                pf.y = (float)world_y;
                pf.z = 0;
                return pf;
            }

        }
        #region CsGL - Plot Here
        public void PlotGL()
        {
            try
            {
                lock (matrixLock)
                {
                    ThisRot.get_Renamed(matrix);
                }
                ////m_Camera.setLook();
                GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT); // Clear screen and DepthBuffer
                GL.glPushMatrix();                  // NEW: Prepare Dynamic Transform
                GL.glMultMatrixf(matrix);
                #region plot something
                this.display();
                #endregion plot something
                GL.glPopMatrix(); // NEW: Unapply Dynamic Transform
                GL.glFlush();     // Flush the GL Rendering Pipeline
                this.Invalidate();
                //m_Camera.setViewByMouse();

            }
            catch
            {
                return;
            }

        }
        public void PlotGLKey()
        {
            try
            {
                GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT); // Clear screen and DepthBuffer
                GL.glPushMatrix();                  // NEW: Prepare Dynamic Transform
                GL.glMultMatrixf(matrix);
                #region plot something
                this.display();
                #endregion plot something
                GL.glPopMatrix(); // NEW: Unapply Dynamic Transform
                GL.glFlush();     // Flush the GL Rendering Pipeline
                this.Invalidate();
            }
            catch (Exception e)
            {
                return;
            }

        }
        public void plot_line_shadow()
        {
            try
            {
                GL.glPushMatrix();                  // NEW: Prepare Dynamic Transform
                #region plot something
                GLU.glPointSize(10);
                GLU.glColor3f(1.0f, 0.0f, 0.0f);
                GLU.glBegin(GLU.GL_POINTS);
                GLU.glVertex3f(p_tags.x, p_tags.y, p_tags.z);
                GLU.glEnd();
                #endregion plot something
                GL.glPopMatrix(); // NEW: Unapply Dynamic Transform
                GL.glFlush();     // Flush the GL Rendering Pipeline
                this.Invalidate();
            }
            catch (Exception e)
            {
                return;
            }

        }
        private void display()
        {
            util u1 = new util();
            #region yuandian
            GLU.glColor3f(1.0f, 0.0f, 0.0f);
            GLU.glPointSize(10);
            GLU.glBegin(GLU.GL_POINTS);
            GLU.glVertex3f(0, 0, 0);
            GLU.glEnd();

            //GLU.glPointSize(3);
            //GLU.glColor3f(1.0f, 0.0f, 0.0f);
            //GLU.glBegin(GLU.GL_LINES);
            //GLU.glVertex3f(0, 0, 0);
            //GLU.glVertex3f(0, 1, 0);
            //GLU.glEnd();

            //GLU.glBegin(GLU.GL_LINES);
            //GLU.glVertex3f(0, 0, 0);
            //GLU.glVertex3f(1, 0, 0);
            //GLU.glEnd();

            //GLU.glBegin(GLU.GL_LINES);
            //GLU.glVertex3f(0, 0, 0);
            //GLU.glVertex3f(0, 0, 1);
            //GLU.glEnd();
            #endregion yuandian
            #region po_tags
            if (p_tags != null)
            {
                if (p_tags.key == 1)
                {
                    GLU.glPointSize(10);
                    GLU.glColor3f(1.0f, 0.0f, 0.0f);
                    GLU.glBegin(GLU.GL_POINTS);
                    GLU.glVertex3f(p_tags.x, p_tags.y, p_tags.z);
                    GLU.glEnd();
                }
                else
                {
                    GLU.glPointSize(10);
                    GLU.glColor3f(1.0f, 0.0f, 0.0f);
                    GLU.glBegin(GLU.GL_POINTS);
                    GLU.glVertex3f(p_tags.x, p_tags.y, p_tags.z);
                    GLU.glEnd();
                }
            }
            #endregion p_tags
            #region pl
            Node<PO> nt = pl.Head;
            while (nt != null&&SHOW_ORDINARY_POINT)
            {
                if(nt.Data.key==5)
                {
                    GLU.glPointSize(5);
                    GLU.glColor3f(0.0f, 0.0f, 1.0f);
                    GLU.glBegin(GLU.GL_POINTS);
                    GLU.glVertex3f(nt.Data.x, nt.Data.y, nt.Data.z);
                    GLU.glEnd();
                }
                else
                {
                    GLU.glPointSize(3);
                    GLU.glColor3f(0.0f, 1.0f, 1.0f);
                    GLU.glBegin(GLU.GL_POINTS);
                    GLU.glVertex3f(nt.Data.x, nt.Data.y, nt.Data.z);
                    GLU.glEnd();
                }

                nt = nt.Next;
            }
            nt = pl_tag.Head;
            while (nt != null&&SHOW_EDIT_MODEL)
            {
                GLU.glPointSize(6);
                GLU.glColor3f(0.0f, 0.4f, 1.0f);
                GLU.glBegin(GLU.GL_POINTS);
                GLU.glVertex3f(nt.Data.x, nt.Data.y, nt.Data.z);
                GLU.glEnd();                
                nt = nt.Next;
            }
            nt = model_special_pl.Head;
            while (nt != null&&SHOW_EDIT_MODEL)
            {
                GLU.glPointSize(10);
                GLU.glColor3f(1.0f, 1.0f, 0.0f);
                GLU.glBegin(GLU.GL_POINTS);
                GLU.glVertex3f(nt.Data.x, nt.Data.y, nt.Data.z);
                GLU.glEnd();
                nt = nt.Next;
            }
            #endregion pl
            #region ll
            Node<LINE> ln = ll.Head;
            while (ln != null&&SHOW_ORDINARY_LL)
            {
                GLU.glColor3f(0.0f, 1.0f, 1.0f);
                GLU.glBegin(GLU.GL_LINES);
                GLU.glVertex3f(ln.Data.p1.x, ln.Data.p1.y, ln.Data.p1.z);
                GLU.glVertex3f(ln.Data.p2.x, ln.Data.p2.y, ln.Data.p2.z);
                GLU.glEnd();
                ln = ln.Next;
            }
            ln = ll_triangle.Head;
            while (ln != null && SHOW_TRIANGLE)
            {
                GLU.glColor3f(0.0f, 1.0f, 1.0f);
                GLU.glBegin(GLU.GL_LINES);
                GLU.glVertex3f(ln.Data.p1.x, ln.Data.p1.y, ln.Data.p1.z);
                GLU.glVertex3f(ln.Data.p2.x, ln.Data.p2.y, ln.Data.p2.z);
                GLU.glEnd();
                ln = ln.Next;
            }
            ln = model_boundary_ll.Head;//模型的边界
            while (ln != null&&SHOW_MODEL_BOUNDARY_CONTROL)
            {
                GLU.glColor3f(1.0f, 0.9f, 0.0f);
                GLU.glBegin(GLU.GL_LINES);
                GLU.glVertex3f(ln.Data.p1.x, ln.Data.p1.y, ln.Data.p1.z);
                GLU.glVertex3f(ln.Data.p2.x, ln.Data.p2.y, ln.Data.p2.z);
                GLU.glEnd();
                ln = ln.Next;
            }
            ln = ll_pebi.Head;
            while (ln != null&&SHOW_PEBI )
            {
                if(ln.Data.Length()>20)
                GLU.glColor3f(1.0f, 1.0f, 0.0f);
                else
                    GLU.glColor3f(1.0f, 0.0f, 0.0f);
                GLU.glBegin(GLU.GL_LINES);
                GLU.glVertex3f(ln.Data.p1.x, ln.Data.p1.y, ln.Data.p1.z);
                GLU.glVertex3f(ln.Data.p2.x, ln.Data.p2.y, ln.Data.p2.z);
                GLU.glEnd();
                ln = ln.Next;
            }
            ln = ll_tag.Head;
            while (ln != null&&SHOW_EDIT_MODEL)
            {
                if (ln.Data.Length() > 20)
                    GLU.glColor3f(1.0f, 0.0f, 1.0f);
                else
                    GLU.glColor3f(1.0f, 0.0f, 0.0f);
                GLU.glBegin(GLU.GL_LINES);
                GLU.glVertex3f(ln.Data.p1.x, ln.Data.p1.y, ln.Data.p1.z);
                GLU.glVertex3f(ln.Data.p2.x, ln.Data.p2.y, ln.Data.p2.z);
                GLU.glEnd();
                ln = ln.Next;
            }
            #endregion ll
            #region sl
            Node<SURFACE> sn = sl_tag.Head;
            while (sn != null)
            {
                GLU.glColor3f(1.0f, 0.0f, 0.0f);
                GLU.glBegin(GLU.GL_LINE_LOOP);
                GLU.glVertex3f(sn.Data.p1.x, sn.Data.p1.y, sn.Data.p1.z);
                GLU.glVertex3f(sn.Data.p2.x, sn.Data.p2.y, sn.Data.p2.z);
                GLU.glVertex3f(sn.Data.p3.x, sn.Data.p3.y, sn.Data.p3.z);
                GLU.glEnd();
                sn = sn.Next;
            }
            if (l_tag != null)
            {
                GLU.glColor3f(1.0f, 0.0f, 0.0f);
                GLU.glBegin(GLU.GL_LINES);
                GLU.glVertex3f(l_tag.p1.x, l_tag.p1.y, l_tag.p1.z);
                GLU.glVertex3f(l_tag.p2.x, l_tag.p2.y, l_tag.p2.z);
                GLU.glEnd();
            }
            #endregion sl
        }
        public void reset()
        {
            lock (matrixLock)
            {
                LastRot.setIdentity();                                // Reset Rotation
                ThisRot.setIdentity();                                // Reset Rotation
                GLU.glLoadIdentity();
            }

            this.PlotGL();
        }
        #endregion CsGL
        #region Mouse Control
        private void startDrag(Point MousePt)
        {
            lock (matrixLock)
            {
                LastRot.set_Renamed(ThisRot); // Set Last Static Rotation To Last Dynamic One
            }
            arcBall.click(MousePt); // Update Start Vector And Prepare For Dragging

            mouseStartDrag = MousePt;

        }
        private void drag(Point MousePt)
        {
            Quat4f ThisQuat = new Quat4f();

            arcBall.drag(MousePt, ThisQuat); // Update End Vector And Get Rotation As Quaternion

            lock (matrixLock)
            {
                //lock 的目的很明确：就是不想让别人使用这段代码，
                //体现在多线程情况下，只允许当前线程执行该代码区域，
                //其他线程等待直到该线程执行结束；这样可以多线程避免同时使用某一方法造成数据混乱。
                ThisRot.Rotation = ThisQuat; // Convert Quaternion Into Matrix3fT
                ThisRot.mul(ThisRot, LastRot); // Accumulate Last Rotation Into This One
            }
        }
        private void drag_Transfer(Point MousePt)
        {
            Quat4f ThisQuat = new Quat4f();

            arcBall.drag(MousePt, ThisQuat); // Update End Vector And Get Rotation As Quaternion

            lock (matrixLock)
            {
                //lock 的目的很明确：就是不想让别人使用这段代码，
                //体现在多线程情况下，只允许当前线程执行该代码区域，
                //其他线程等待直到该线程执行结束；这样可以多线程避免同时使用某一方法造成数据混乱。
                ThisRot.Transfer = ThisQuat; // Convert Quaternion Into Matrix3fT
                ThisRot.mul(ThisRot, LastRot); // Accumulate Last Rotation Into This One
            }
        }
        public void glOnMouseMove(object sender, MouseEventArgs e)
        {
            Point tempAux = new Point(e.X, e.Y);
            PO vec = new PO();
            mouseEndDrag = tempAux;
            if (isLeftDrag)
            {
                vec = Map_to_Space(e.X, e.Y);
                if (p_tags != null)
                {
                    if (ADD_MODEL_fault || p_tags.key == 1 || p_tags.key == 2 || ADD_MODEL_hwell)
                    {
                        l_tag = new LINE(p_tags, vec);
                        this.PlotGLKey();
                    }

                }
            }

            if (isRightDrag)
            {
            }

            if(isMiddleDrag)
            {
            }
            
        }
        public void glOnLeftMouseDown(object sender, MouseEventArgs e)
        {
            isLeftDrag = true;
            PO pt = Map_to_Space(e.X, e.Y);
            p_tags = pt;
            this.PlotGLKey();
            if (ADD_MODEL_vwell)
            {
                Clear();
                c.Create_Vertical_Well(pt);
                c.START_form(this);
                ADD_MODEL_vwell = false;
                Delaunay_2D de = new Delaunay_2D();
                de.GetOGL(this);
                de.Get_initialpl(c.initial_pl);
                de.Get_initial_center(c.center);
                de.GetPL(c.Get_p_l());
                de.Start();
                this.PlotGLKey();
            }
            else if (ADD_MODEL_fault) ;
            else if (ADD_MODEL_hwell) ;
            else
                Seek_Special_PO(pt);      
        }
        public void glOnLeftMouseUp(object sender, MouseEventArgs e)
        {
            isLeftDrag = false;
            PO pt=Map_to_Space(e.X, e.Y);
            if (p_tags != null)
            {
                if (ADD_MODEL_fault)
                {
                    LINE l0 = new LINE(p_tags,pt);
                    Clear();
                    c.Create_fault(l0);
                    c.START_form(this);
                    p_tags = null;
                    ADD_MODEL_fault = false;
                    l_tag = null;
                    Delaunay_2D de = new Delaunay_2D();
                    de.GetOGL(this);
                    de.Get_initialpl(c.initial_pl);
                    de.Get_initial_center(c.center);
                    de.GetPL(c.Get_p_l());
                    de.Start();
                    this.PlotGLKey();
                }
                else if (ADD_MODEL_hwell)
                {
                    LINE l0 = new LINE(p_tags, pt);
                    Clear();
                    c.Create_h_well2d(l0);
                    c.START_form(this);
                    p_tags = null;
                    ADD_MODEL_hwell = false;
                    l_tag = null;
                    Delaunay_2D de = new Delaunay_2D();
                    de.GetOGL(this);
                    de.Get_initialpl(c.initial_pl);
                    de.Get_initial_center(c.center);
                    de.GetPL(c.Get_p_l());
                    de.Start();
                    this.PlotGLKey();
                }
                else if (p_tags.key == 1||p_tags.key==2)
                {
                    p_tags.x = pt.x;
                    p_tags.y = pt.y;
                    Clear();
                    c.START_form(this);
                    Delaunay_2D de = new Delaunay_2D();
                    de.GetOGL(this);
                    de.Get_initialpl(c.initial_pl);
                    de.Get_initial_center(c.center);
                    de.GetPL(c.Get_p_l());
                    de.Start();
                    this.PlotGLKey();
                }
                this.PlotGLKey();
             }
        }
        public void getScale(float scalei)
        {
            LastScale = scalei;
        }
        private void glOnRightMouseDown(object sender, MouseEventArgs e)
        {
            isRightDrag = true;
        }
        private void glOnRightMouseUp(object sender, MouseEventArgs e)
        {
            isRightDrag = false;
            //this.reset();
        }
        public void glOnMiddleMouseDown(object sender, MouseEventArgs e)
        {
            isMiddleDrag = true;
        }
        public void glOnMiddleMouseUp(object sender, MouseEventArgs e)
        {

            isMiddleDrag = false;
        }
        public void glOnMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                angle = angle + 1;
                if (angle > 150)
                    angle = 150;
            }
            if(e.Delta<0)
            {
                angle = angle - 1;
                if (angle < 5)
                    angle = 5;
            }
            GL.glLoadIdentity();
            GL.gluPerspective(angle, 1, 20.0f, 40.0f);
            GL.gluLookAt(left_right, top_down, 10.0f, left_right, top_down, 0.0f, 0.0f, 1.0f, 0.0f);
            this.PlotGLKey();
        }
        #endregion
        #region Key Control
        public void glOnKeyDown(object Sender, KeyEventArgs kea)
        {
            //if escape was pressed, exit the application
            if (kea.KeyCode == Keys.Escape)
            {
                this.Parent.Dispose();
            }
            //if R was pressed, reset
            if (kea.KeyCode == Keys.R)
            {
                this.reset();
            }
            if (kea.KeyCode == Keys.W)
            {
                top_down=top_down+1;
                GL.glLoadIdentity();
                GL.gluPerspective(angle, 1, 20.0f, 40.0f);
                GL.gluLookAt(left_right, top_down, 10.0f, left_right, top_down, 0.0f, 0.0f, 1.0f, 0.0f);
                this.PlotGLKey();
            }
            if (kea.KeyCode == Keys.S)
            {
                top_down=top_down-1;
                GL.glLoadIdentity();
                GL.gluPerspective(angle, 1, 20.0f, 40.0f);
                GL.gluLookAt(left_right, top_down, 10.0f, left_right, top_down, 0.0f, 0.0f, 1.0f, 0.0f);
                this.PlotGLKey();
            }
            if (kea.KeyCode == Keys.A)
            {
                left_right  = left_right - 1;
                GL.glLoadIdentity();
                GL.gluPerspective(angle, 1, 20.0f, 40.0f);
                GL.gluLookAt(left_right, top_down, 10.0f, left_right, top_down, 0.0f, 0.0f, 1.0f, 0.0f);
                this.PlotGLKey();
            }
            if (kea.KeyCode == Keys.D)
            {
                left_right = left_right + 1;
                GL.glLoadIdentity();
                GL.gluPerspective(angle, 1, 20.0f, 40.0f);
                GL.gluLookAt(left_right, top_down, 10.0f, left_right, top_down, 0.0f, 0.0f, 1.0f, 0.0f);
                this.PlotGLKey();
            }
            if (kea.KeyCode == Keys.F11)
            {

                angle = angle - 1;
                GL.glLoadIdentity();
                GL.gluPerspective(angle, 1, 20.0f, 40.0f);
                GL.gluLookAt(left_right, top_down, 10.0f, left_right, top_down, 0.0f, 0.0f, 1.0f, 0.0f);
                this.PlotGLKey();
            }
            if (kea.KeyCode == Keys.F12)
            {
                angle = angle + 1;
                GL.glLoadIdentity();
                GL.gluPerspective(angle, 1, 20.0f, 40.0f);
                GL.gluLookAt(left_right, top_down, 10.0f, left_right, top_down, 0.0f, 0.0f, 1.0f, 0.0f);
                this.PlotGLKey();
            }
        }
        #endregion Key Control
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
    #endregion OpenGL Control

    #region MouseControl
    public class MouseControl
    {
        protected Control newCtrl;
        protected MouseButtons FinalClick;

        public event EventHandler LeftClick;
        public event EventHandler RightClick;
        public event EventHandler MiddleClick;

        public event MouseEventHandler LeftMouseDown;
        public event MouseEventHandler LeftMouseUp;
        public event MouseEventHandler RightMouseDown;
        public event MouseEventHandler RightMouseUp;
        public event MouseEventHandler MiddleMouseDown;
        public event MouseEventHandler MiddleMouseUp;
        public event MouseEventHandler MouseWheel;
        public Control Control
        {
            get { return newCtrl; }
            set
            {
                newCtrl = value;
                Initialize();
            }
        }

        public MouseControl()
        {
        }

        public MouseControl(Control ctrl)
        {
            Control = ctrl;
        }

        public void AddControl(Control ctrl)
        {
            Control = ctrl;
        }

        protected virtual void Initialize()
        {
            newCtrl.Click += new EventHandler(OnClick);
            newCtrl.MouseDown += new MouseEventHandler(OnMouseDown);
            newCtrl.MouseUp += new MouseEventHandler(OnMouseUp);
            newCtrl.MouseWheel+=new MouseEventHandler(OnMouseWheel);
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            MouseWheel(sender,e);
        }

        private void OnClick(object sender, EventArgs e)
        {
            switch (FinalClick)
            {
                case MouseButtons.Left:
                    if (LeftClick != null)
                    {
                        LeftClick(sender, e);
                    }
                    break;

                case MouseButtons.Right:
                    if (RightClick != null)
                    {
                        RightClick(sender, e);
                    }
                    break;
                case MouseButtons.Middle:
                    if (MiddleClick != null)
                    {
                        MiddleClick(sender, e);
                    }
                    break;
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            FinalClick = e.Button;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        if (LeftMouseDown != null)
                        {
                            LeftMouseDown(sender, e);
                        }
                        break;
                    }

                case MouseButtons.Right:
                    {
                        if (RightMouseDown != null)
                        {
                            RightMouseDown(sender, e);
                        }
                        break;
                    }
                case MouseButtons.Middle:
                    {
                        if (MiddleMouseDown != null)
                        {
                            MiddleMouseDown(sender, e);
                        }
                        break;
                    }
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        if (LeftMouseUp != null)
                        {
                            LeftMouseUp(sender, e);
                        }
                        break;
                    }

                case MouseButtons.Right:
                    {
                        if (RightMouseUp != null)
                        {
                            RightMouseUp(sender, e);
                        }
                        break;
                    }
                case MouseButtons.Middle:
                    {
                        if (MiddleMouseUp != null)
                        {
                            MiddleMouseUp(sender, e);
                        }
                        break;
                    }
            }
        }
    }
    #endregion MouseControl
}
