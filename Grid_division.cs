using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace arcball
{
    class Delaunay
    {
       private float soliangle1,soliangle2;//用于选择立体角最大的点
       private PO p0, p1, p2, p3, pi, pj, pk;//p0,p1,p2,p3代表长方体的四个顶点
       private PO pg;//代表最后选择的点
       private float x1,x2,x3;//x1,x2,x3代表沿三个方向的细分数
       private LinkList<PO> pl;//表示当前所处理的点集
       private int loc=0;//表示当前搜索到那个点
        //new algorithm
       private LinkList<LINE> pre_l;//表示当前正在被处理的线链
       private LinkList<SURFACE> sl;//表示画图的面链
       private LinkList<PO> bad_pl;//表示当前的线不能选的点集
       private LinkList<LINE> ll;//表示用于画图的线链
       protected OGL PAINT = null;//用于画图的对象
       private PO pkey;//表示当前正在处理的点
      //
       private LinkList<PO> work_pl;//表示当前工作的链集
       public Delaunay()
       {      
           //初始化工作边集
           work_pl = new LinkList<PO>();
           sl = new LinkList<SURFACE>();
           pre_l = new LinkList<LINE>();
           bad_pl = new LinkList<PO>();
           ll = new LinkList<LINE>();
       }
       public void GetOGL(OGL pa)
       {
           //获取具备画图功能的对象
           PAINT = pa;
       }
       public void Show_sl()
       {
           Node<SURFACE > sn;
           sn = sl.Head;
           while (sn != null)
           {
               PAINT.GetSL(sn.Data);
               sn = sn.Next;
           }
           sl.Clear();
       }
       public void Initial(model3d m)
       {
           //m从生成某个模型的点集中获取包含一个初始面的点集
           util u1 = new util();
           SURFACE s;
           PO p0,p1,p2;
           LINE l1, l2, l3;
           //获取初始面的三个顶点和一个初始面
           p0 = m.pi;
           p1 = m.pj;
           p2 = m.pk;
           s = new SURFACE(p0, p1, p2);
           //加入初始的点，构成初始点集
           work_pl.Insert(p0);
           work_pl.Insert(p1);
           work_pl.Insert(p2);
           //产生初始面的三个边
           l1 = new LINE(p0,p1);
           l2 = new LINE(p0,p2);
           l3 = new LINE(p1,p2);
           //将三条边加到面中
           s.l1 = l1;
           s.l2 = l2;
           s.l3 = l3;
           //为每个边加入初始的面集
           l1.sl.Insert(s);
           l2.sl.Insert(s);
           l3.sl.Insert(s);
           //为每个点加入初始的边集
           p0.ll.Insert(l1);
           p0.ll.Insert(l2);
           p1.ll.Insert(l1);
           p1.ll.Insert(l3);
           p2.ll.Insert(l2);
           p2.ll.Insert(l3);
           pl = m.Get_get_p_l();        //获取模型的点数据，为一个点链      
           sl.Insert(s);
           PAINT.GetLL(l1);
           PAINT.GetLL(l2);
           PAINT.GetLL(l3);
       }
       #region method
       private void Set_direction(SURFACE s, PO p0)
       {
           /*
            * 根据p0与s的相互关系，决定s的某个方向上是否有效
            * 设定为无效，则对应的标志为
            */
           util u1 = new util();
           float f;
           f = u1.Direct(s, p0);
           //u1.InFile("e:/info.txt", "-----start Set_direction--------");
           //s.ToPrint();
           //u1.InFile("e:/info.txt", p0);
           //u1.InFile("e:/info.txt", f);
           //u1.InFile("e:/info.txt", "-----end Set_direction--------");
           if (f > 0)
               s.positive = false;
           else if (f < 0)
               s.negtive = false;
       }
       private void state_of_surface(SURFACE s)
       {
           util u1 = new util();
           //if (s.positive)
           //    u1.InFile("e:/info.txt", "positive hasn't been searched");
           //else
           //    u1.InFile("e:/info.txt", "positive has been searched");
           //if (s.negtive)
           //    u1.InFile("e:/info.txt", "negetive hasn't been searched");
           //else
           //    u1.InFile("e:/info.txt", "negetive has been searched");

       }
       private SURFACE new_Surface(PO p1, PO p2, PO p3, PO p0)
       {
           util u1 = new util();
           SURFACE s;
           s = new SURFACE(p1,p2,p3);
           Set_direction(s,p0);
           return s;
       }
       private void ShowSur(PO p)
       {
           Node<SURFACE> st=p.sl.Head;
           while(st!=null)
           {
               //st.Data.ToPrint();
               st = st.Next;
           }
       }
       private bool Two_side_Existed(SURFACE s1, SURFACE s2,ref PO px1,ref PO px2)
       {
           #region s1.p1==s2.p1
           if (s1.p1 == s2.p1)
           {
               if (s1.p2 == s2.p2)
               {
                   px1 = s1.p3;
                   px2 = s2.p3;
                   return true;
               }
               else if (s1.p2 == s2.p3)
               {
                   px1 = s1.p3;
                   px2 = s2.p2;
                   return true;
               }
               else if (s1.p3 == s2.p2)
               {
                   px1 = s1.p2;
                   px2 = s2.p3;
                   return true;
               }
               else if (s1.p3 == s2.p3)
               {
                   px1 = s1.p2;
                   px2 = s2.p2;
                   return true;
               }
               return false;
           }
           #endregion s1.p1==s2.p1
           #region s1.p1==s2.p2
           if (s1.p1 == s2.p2)
           {
               if (s1.p2 == s2.p1)
               {
                   px1 = s1.p3;
                   px2 = s2.p3;
                   return true;
               }
               else if (s1.p2 == s2.p3)
               {
                   px1 = s1.p3;
                   px2 = s2.p1;
                   return true;
               }
               else if (s1.p3 == s2.p1)
               {
                   px1 = s1.p2;
                   px2 = s2.p3;
                   return true;
               }
               else if (s1.p3 == s2.p3)
               {
                   px1 = s1.p2;
                   px2 = s2.p1; 
                   return true;
               }
               return false;
           }
           #endregion s1.p1==s2.p2
           #region s1.p1==s2.p3
           if (s1.p1 == s2.p3)
           {
               if (s1.p2 == s2.p1)
               {
                   px1 = s1.p3;
                   px2 = s2.p2;
                   return true;
               }
               else if (s1.p2 == s2.p2)
               {
                   px1 = s1.p3;
                   px2 = s2.p1;
                   return true;
               }
               else if (s1.p3 == s2.p1)
               {
                   px1 = s1.p2;
                   px2 = s2.p2;
                   return true;
               }
               else if (s1.p3 == s2.p2)
               {
                   px1 = s1.p2;
                   px2 = s2.p1;
                   return true;
               }
               return false;
           }
           #endregion s1.p1==s2.p3
           #region s1.p2==s2.p1
           if (s1.p2 == s2.p1)
           {
               if (s1.p3 == s2.p2)
               {
                   px1 = s1.p1;
                   px2 = s2.p3;
                   return true;
               }
               else if (s1.p3 == s2.p3)
               {
                   px1 = s1.p1;
                   px2 = s2.p2;
                   return true;
               }
               else if (s1.p1 == s2.p2)
               {
                   px1 = s1.p3;
                   px2 = s2.p3;
                   return true;
               }
               else if (s1.p1 == s2.p3)
               {
                   px1 = s1.p3;
                   px2 = s2.p2;
                   return true;
               }
               return false;
           }
           #endregion s1.p2==s2.p1
           #region s1.p2==s2.p2
           if (s1.p2 == s2.p2)
           {
               if (s1.p3 == s2.p1)
               {
                   px1 = s1.p1;
                   px2 = s2.p3;
                   return true;
               }
               else if (s1.p3 == s2.p3)
               {
                   px1 = s1.p1;
                   px2 = s2.p1;
                   return true;
               }
               else if (s1.p1 == s2.p1)
               {
                   px1 = s1.p3;
                   px2 = s2.p3;
                   return true;
               }
               else if (s1.p1 == s2.p3)
               {
                   px1 = s1.p3;
                   px2 = s2.p1;
                   return true;
               }
               return false;
           }
           #endregion s1.p2==s2.p2
           #region s1.p2==s2.p3
           if (s1.p2 == s2.p3)
           {
               if (s1.p3 == s2.p1)
               {
                   px1 = s1.p1;
                   px2 = s2.p2;
                   return true;
               }
               else if (s1.p3 == s2.p2)
               {
                   px1 = s1.p1;
                   px2 = s2.p1;
                   return true;
               }
               else if (s1.p1 == s2.p1)
               {
                   px1 = s1.p3;
                   px2 = s2.p2;
                   return true;
               }
               else if (s1.p1 == s2.p2)
               {
                   px1 = s1.p3;
                   px2 = s2.p1;
                   return true;
               }
               return false;
           }
           #endregion s1.p2==s2.p3
           #region s1.p3==s2.p1
           if (s1.p3 == s2.p1)
           {
               if (s1.p2 == s2.p2)
               {
                   px1 = s1.p1;
                   px2 = s2.p3;
                   return true;
               }
               else if (s1.p2 == s2.p3)
               {
                   px1 = s1.p1;
                   px2 = s2.p2;
                   return true;
               }
               else if (s1.p1 == s2.p2)
               {
                   px1 = s1.p2;
                   px2 = s2.p3;
                   return true;
               }
               else if (s1.p1 == s2.p3)
               {
                   px1 = s1.p2;
                   px2 = s2.p2;
                   return true;
               }
               return false;
           }
           #endregion s1.p3==s2.p1
           #region s1.p3==s2.p2
           if (s1.p3 == s2.p2)
           {
               if (s1.p2 == s2.p1)
               {
                   px1 = s1.p1;
                   px2 = s2.p3;
                   return true;
               }
               else if (s1.p2 == s2.p3)
               {
                   px1 = s1.p1;
                   px2 = s2.p1;
                   return true;
               }
               else if (s1.p1 == s2.p1)
               {
                   px1 = s1.p2;
                   px2 = s2.p3;
                   return true;
               }
               else if (s1.p1 == s2.p3)
               {
                   px1 = s1.p2;
                   px2 = s2.p1;
                   return true;
               }
               return false;
           }
           #endregion s1.p3==s2.p2
           #region s1.p3==s2.p3
           if (s1.p3 == s2.p3)
           {
               if (s1.p2 == s2.p1)
               {
                   px1 = s1.p1;
                   px2 = s2.p2;
                   return true;
               }
               else if (s1.p2 == s2.p2)
               {
                   px1 = s1.p1;
                   px2 = s2.p1;
                   return true;
               }
               else if (s1.p1== s2.p1)
               {
                   px1 = s1.p2;
                   px2 = s2.p2;
                   return true;
               }
               else if (s1.p1 == s2.p2)
               {
                   px1 = s1.p2;
                   px2 = s2.p1;
                   return true;
               }
               return false;
           }
           #endregion s1.p3==s2.p3
           return false;
       }
       private PO Judge_Conface(SURFACE s0,Node<SURFACE> sn)
       {
           PO px1, px0;
           px1 = null;
           px0 = null;
           Node<SURFACE> st = sn.Next;
           util u1 =new util();
           u1.InFile(u1.infopath ,"start Judge_Conface");
           while (st!=null)
           {
               if (Two_side_Existed(st.Data, s0, ref px1, ref px0))
               {
                   SearchD(st.Data);
                   if (pg == px0)
                   {
                       u1.InFile(u1.infopath, "find it");

                       return px1;
                   }
               }
               st = st.Next;
           }
           u1.InFile(u1.infopath, "end Judge_Conface");
           return null;
       }
       private PO IsExistPo(PO p0)
       {
           Node<PO> pt = work_pl.Head;
           while (pt!=null)
           {
               if (pt.Data == p0)
               {
                   return pt.Data;
               }
           }
           return null;
       }
       private int GetPara(SURFACE s1,SURFACE s2,PO pgt,ref PO p0,ref PO p1,ref PO p2,ref SURFACE sx)
       {
           util u1 = new util();
           #region s1.p1 == pgt
           if (s1.p1 == pgt)
           {
               //u1.InFile("e:/state.txt","--s1.p1--");
               #region s1.p2==s2.p1
               if (s1.p2 == s2.p1)
               {
                   if (s1.p3 == s2.p2)
                   {
                       p1 = s2.p1;
                       p2 = s2.p2;
                       p0 = s2.p3;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p3 == s2.p3)
                   {
                       p1 = s2.p1;
                       p2= s2.p3;
                       p0 = s2.p2;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p2==s2.p1
               #region s1.p2==s2.p2
               if (s1.p2 == s2.p2)
               {
                   if (s1.p3 == s2.p1)
                   {
                       p1 = s2.p1;
                       p2 = s2.p2;
                       p0 = s2.p3;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p3 == s2.p3)
                   {
                       p1 = s2.p2;
                       p2 = s2.p3;
                       p0 = s2.p1;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p2==s2.p2
               #region s1.p2==s2.p3
               if (s1.p2 == s2.p3)
               {
                   if (s1.p3 == s2.p1)
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p3 == s2.p2)
                   {
                       p1 = s2.p2;
                       p2 = s2.p3;
                       p0 = s2.p1;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p2==s2.p3
               #region s1.p3==s2.p1
               if (s1.p3 == s2.p1)
               {
                   if (s1.p2 == s2.p2)
                   {
                       p1 = s2.p1;
                       p2 = s2.p2;
                       p0 = s2.p3;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p2 == s2.p3)
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p3==s2.p1
               #region s1.p3==s2.p2
               if (s1.p3 == s2.p2)
               {
                   if (s1.p2== s2.p1)
                   {
                       p1 = s2.p1;
                       p2 = s2.p2;
                       p0 = s2.p3;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p2 == s2.p3)
                   {
                       p1 = s2.p2;
                       p2 = s2.p3;
                       p0 = s2.p1;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p3==s2.p2
               #region s1.p3==s2.p3
               if (s1.p3 == s2.p3)
               {
                   if (s1.p2 == s2.p1)
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p2 == s2.p2)
                   {
                       p1 = s2.p2;
                       p2 = s2.p3;
                       p0 = s2.p1;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p3==s2.p3
           }
           #endregion s1.p1==pgt
           #region s1.p2 ==pgt
           if (s1.p2 == pgt)
           {
               //u1.InFile("e:/state.txt", "--s1.p2--");
               #region s1.p1==s2.p1
               if (s1.p1 == s2.p1)
               {
                   if (s1.p3 == s2.p2)
                   {
                       p1 = s2.p1;
                       p2 = s2.p2;
                       p0 = s2.p3;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p3 == s2.p3)
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p1==s2.p1
               #region s1.p1==s2.p2
               if (s1.p1 == s2.p2)
               {
                   if (s1.p3 == s2.p1)
                   {
                       p1 = s2.p1;
                       p2 = s2.p2;
                       p0 = s2.p3;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p3 == s2.p3)
                   {
                       p1 = s2.p2;
                       p2 = s2.p3;
                       p0 = s2.p1;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p2==s2.p2
               #region s1.p1==s2.p3
               if (s1.p1 == s2.p3)
               {
                   if (s1.p3 == s2.p1)
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p3 == s2.p2)
                   {
                       p1 = s2.p2;
                       p2 = s2.p3;
                       p0 = s2.p1;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p2==s2.p3
               #region s1.p3==s2.p1
               if (s1.p3 == s2.p1)
               {
                   if (s1.p1 == s2.p2)
                   {
                       p1 = s2.p1;
                       p2 = s2.p2;
                       p0 = s2.p3;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p1== s2.p3)
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p3==s2.p1
               #region s1.p3==s2.p2
               if (s1.p3 == s2.p2)
               {
                   if (s1.p1 == s2.p1)
                   {
                       p1 = s2.p1;
                       p2 = s2.p2;
                       p0 = s2.p3;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p1 == s2.p3)
                   {
                       p1 = s2.p2;
                       p2 = s2.p3;
                       p0 = s2.p1;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p3==s2.p2
               #region s1.p3==s2.p3
               if (s1.p3 == s2.p3)
               {
                   if (s1.p1 == s2.p1)
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p1 == s2.p2)
                   {
                       p1 = s2.p2;
                       p2 = s2.p3;
                       p0 = s2.p1;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p3==s2.p3
           }
           #endregion s1.p2==pgt
           #region s1.p3 == pgt
           if (s1.p3 == pgt)
           {
               //u1.InFile("e:/state.txt", "--s1.p3--");
               #region s1.p1==s2.p1
               if (s1.p1 == s2.p1)
               {
                   //u1.InFile("e:/state.txt", "--s1.p31--");
                   if (s1.p2 == s2.p2)
                   {
                       p1 = s2.p1;
                       p2 = s2.p2;
                       p0 = s2.p3;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p2 == s2.p3)
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p1==s2.p1
               #region s1.p1==s2.p2
               if (s1.p1 == s2.p2)
               {
                   //u1.InFile("e:/state.txt", "--s1.p32--");
                   if (s1.p2== s2.p1)
                   {
                       //u1.InFile("e:/state.txt", "--s1.p321--");
                       p1 = s2.p1;
                       p2 = s2.p2;
                       p0 = s2.p3;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p2 == s2.p3)
                   {
                       //u1.InFile("e:/state.txt", "--s1.p322--");
                       p1 = s2.p2;
                       p2 = s2.p3;
                       p0 = s2.p1;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       //u1.InFile("e:/state.txt", "--s1.p323--");
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p2==s2.p2
               #region s1.p1==s2.p3
               if (s1.p1 == s2.p3)
               {
                   //u1.InFile("e:/state.txt", "--s1.p33--");
                   if (s1.p2 == s2.p1)
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p2 == s2.p2)
                   {
                       p1 = s2.p2;
                       p2 = s2.p3;
                       p0 = s2.p1;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p2==s2.p3
               #region s1.p2==s2.p1
               if (s1.p2 == s2.p1)
               {
                   //u1.InFile("e:/state.txt", "--s1.p34--");
                   if (s1.p1 == s2.p2)
                   {
                       p1 = s2.p1;
                       p2 = s2.p2;
                       p0 = s2.p3;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p1 == s2.p3)
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p3==s2.p1
               #region s1.p2==s2.p2
               if (s1.p2 == s2.p2)
               {
                   //u1.InFile("e:/state.txt", "--s1.p35--");
                   if (s1.p1 == s2.p1)
                   {
                       p1 = s2.p1;
                       p2 = s2.p2;
                       p0 = s2.p3;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p1 == s2.p3)
                   {
                       p1 = s2.p2;
                       p2 = s2.p3;
                       p0 = s2.p1;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p3==s2.p2
               #region s1.p2==s2.p3
               if (s1.p2 == s2.p3)
               {
                   //u1.InFile("e:/state.txt", "--s1.p36--");
                   if (s1.p1 == s2.p1)
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = s1;
                       return 2;
                   }
                   else if (s1.p1 == s2.p2)
                   {
                       p1 = s2.p2;
                       p2 = s2.p3;
                       p0 = s2.p1;
                       sx = s1;
                       return 2;
                   }
                   else
                   {
                       p1 = s2.p1;
                       p2 = s2.p3;
                       p0 = s2.p2;
                       sx = null;
                       return 1;
                   }
               }
               #endregion s1.p3==s2.p3
           }
           #endregion s1.p3==pgt
           return 0;
       }
       private int state_of_sp(SURFACE s,PO p0,ref PO px0,ref PO px1,ref PO px2,
           ref SURFACE sx,ref SURFACE sx2,ref PO psx1,ref PO psx2,ref PO psxn)
       {
           //i=2,表示p0与s有两点共面；i=1,表示p0与s有一个点共面
           Node<SURFACE> st;
           PO p0t, p1t, p2t;
           PO p0t2, p1t2, p2t2;
           int two_face_tag=-1;//标志有两个面与当前面相邻
           SURFACE sxt,sxt2;
           util u1 = new util();
           p0t = null;
           p1t = null;
           p2t = null;
           sxt = null;
           p0t2 = null;
           p1t2 = null;
           p2t2 = null;
           sxt2 = null;
           int i = 0,itep=0;
           #region s.p1
           st = s.p1.sl.Head;
           while (st != null)
           {
               i = GetPara(st.Data, s, p0, ref p0t, ref p1t, ref p2t, ref sxt);
               if (i >= 1)
               {
                   px0 = p0t;
                   px1 = p1t;
                   px2 = p2t;
                   sx = sxt;
                   itep = 1;
                   //u1.InFile("e:/state.txt", "--itep=1-");
                   if (i == 2)
                   {
                       if (two_face_tag == 2)
                       {
                           sx2 = sxt;
                           psx2 = p0t;
                           sx = sxt2;
                           psx1 = p0t2;
                           if (p0t2 == p1t)
                               psxn = p2t;
                           else
                               psxn = p1t;
                           return 3;
                       }
                       else
                       {
                           p0t2 = p0t;
                           p1t2 = p1t;
                           p2t2 = p2t;
                           sxt2 = sxt;
                           two_face_tag++;
                       }
                   };
               }
               st = st.Next;
           }
           if (two_face_tag == 1)
           {
               two_face_tag = 0;
           }
           #endregion s.p1
           #region s.p2
           st = s.p2.sl.Head;
           while (st != null)
           {
               i = GetPara(st.Data, s, p0, ref p0t, ref p1t, ref p2t, ref sxt);
               if (i >= 1)
               {
                   px0 = p0t;
                   px1 = p1t;
                   px2 = p2t;
                   sx = sxt;
                   itep = 1;
                   //u1.InFile("e:/state.txt", "--itep=1-");
                   if (i == 2)
                   {
                       if (two_face_tag == 2)
                       {
                           sx2 = sxt;
                           psx2 = p0t;
                           sx = sxt2;
                           psx1 = p0t2;
                           if (p0t2 == p1t)
                               psxn = p2t;
                           else
                               psxn = p1t;
                           return 3;
                       }
                       p0t2 = p0t;
                       p1t2 = p1t;
                       p2t2 = p2t;
                       sxt2 = sxt;
                       two_face_tag++;
                   };
               }
               st = st.Next;
           }
           if (two_face_tag == 1)
           {
               two_face_tag = 0;
           }
           #endregion s.p2
           #region s.p3
           st = s.p3.sl.Head;
           while (st != null)
           {
               i = GetPara(st.Data, s, p0, ref p0t, ref p1t, ref p2t, ref sxt);
               if (i >= 1)
               {
                   px0 = p0t;
                   px1 = p1t;
                   px2 = p2t;
                   sx = sxt;
                   itep = 1;
                   //u1.InFile("e:/state.txt", "--itep=1-");
                   if (i == 2)
                   {
                       if (two_face_tag == 2)
                       {
                           sx2 = sxt;
                           psx2 = p0t;
                           sx = sxt2;
                           psx1 = p0t2;
                           if (p0t2 == p1t)
                               psxn = p2t;
                           else
                               psxn = p1t;
                           return 3;
                       }
                       p0t2 = p0t;
                       p1t2 = p1t;
                       p2t2 = p2t;
                       sxt2 = sxt;
                       two_face_tag++;
                   };
               }
               st = st.Next;
           }
           if (two_face_tag == 1)
           {
               two_face_tag = 0;
           }
           #endregion s.p3
           if (two_face_tag == 0)
           {
               px0 = p0t2;
               px1 = p1t2;
               px2 = p2t2;
               sx = sxt2;
               return 2;
           }
           return itep;
       }
       #endregion method
       private bool Is_inValid_Point(SURFACE s0,SURFACE st,LINE lk)
       {
           
            /*
             * 判断pg是否在合适的位置，如果是返回true,否则返回false
             * s0表示正在处理的面
             * st是与s0相邻的面，lk是它们俩的相邻边
             */
            util u1=new util();
            Node<SURFACE> sn = sl.Head;
            PO pt;//用于记录需要检测的面上的一个点
            pt = s0.GetPd(lk);
            if (pt != null)
                if (u1.Direct(st, pg) * u1.Direct(st, pt) >= 0)
                    return false;
                else
                    return true;
            else
            {
                MessageBox.Show("lk isn't on the face");
                return false;
            }
       }
       private SURFACE Is_Fitting_Point_2(SURFACE s, ref int flag, LINE lk)
       {
           /*
            * s是当前正在处理的面,flag 表示当前情况
            * 检查pg和s的三个点是否在s的三条边的面链中某个面的两侧
            * 如果点pg已存在，则返回相应点
            * lk为当前处理的边
            */
           util u1 = new util();
           PO pd;//保存s0上的点
           LINE l1, l2, l3;//获取s的三条边
           l1 = s.l1;
           l2 = s.l2;
           l3 = s.l3;
           SURFACE s1, s2, s3;//获取在s的边的面链中与s相邻的三个面
           s1 = l1.Get_Surface(s);
           s2 = l2.Get_Surface(s);
           s3 = l3.Get_Surface(s);
           if (s1 != null && Is_inValid_Point(s, s1, lk))
           {
               flag = -1;
               u1.InFile(u1.infopath, "kx1");
               s1.ToString();
               u1.InFile(u1.infopath, "---------------?");
               return null;
           }
           if (s2 != null && Is_inValid_Point(s, s2, lk))
           {
               flag = -1;
               u1.InFile(u1.infopath, "kx2");
               s2.ToString();
               u1.InFile(u1.infopath, "---------------?");
               return null;
           }
           if (s3 != null && Is_inValid_Point(s, s3, lk))
           {
               flag = -1;
               u1.InFile(u1.infopath, "kx3");
               s3.ToString();
               u1.InFile(u1.infopath, "---------------?");
               return null;
           }
           PO pt;//表示可能为当前pg的点
           u1.InFile(u1.infopath, "k0");
           if (pg.selected)
           /*
            * 如果当前的点已被选过，则当前的点是特殊点，需要特殊处理
            */
           {
               u1.InFile(u1.infopath, "k1");
               #region while
               Node<SURFACE> sn = l1.sl.Head;
               //l1.ToString();
               //lk.ToString();
               if (l1 == lk)//如果l1为当前处理的边，则认为pg为普通点
               {
                   u1.InFile(u1.infopath, "l1 == lk");
                   flag = 1;
               }
               if (pg == l1.sl.Head.Data.GetPd(l1))
               {
                   u1.InFile(u1.infopath, "k3");
                   flag = 1;
                   return null;
               }
               if (pg == l1.sl.Last.Data.GetPd(l1))
               {
                   //如果pg在与当前待处理面相邻的某个面上，且为头面或尾面
                   flag = 0;
                   return l1.sl.Last.Data;
               }
               while (sn != null)
               {
                   if (sn.Next != null)
                       if (sn.Next.Data == s)
                       {
                           u1.InFile(u1.infopath, "s11");
                           pt = sn.Data.GetPd(l1);//取得面sn.Data上除lk外的顶点
                           if (pt == pg)
                           //如果当前pt和pg是同一个点，则pg是有效点
                           {
                               if (l1 == lk)//如果l1为当前处理的边，则认为pg为普通点
                               {
                                   flag = 1;
                                   return null;
                               }
                               flag = 0;
                               return sn.Data;
                           }

                       }
                   if (sn.Data == s && sn.Next != null)
                   {
                       u1.InFile(u1.infopath, "s12");
                       pt = sn.Next.Data.GetPd(l1);//取得面sn.Next.Data上除lk外的顶点
                       if (pt == pg)
                       //如果当前pt和pg是同一个点，则pg是有效点
                       {
                           if (l1 == lk)//如果l3为当前处理的边，则认为pg为普通点
                           {
                               flag = 1;
                               return null;
                           }
                           flag = 0;
                           return sn.Next.Data;
                       }
                   }
                   sn = sn.Next;
               }
               u1.InFile(u1.infopath, "k1x");
               sn = l2.sl.Head;
               //l2.ToString();
               //lk.ToString();
               if (l2 == lk)//如果l1为当前处理的边，则认为pg为普通点
               {
                   u1.InFile(u1.infopath, "k5");
                   flag = 1;
               }
               if (pg == l2.sl.Head.Data.GetPd(l2))
               {
                   u1.InFile(u1.infopath, "k6");
                   flag = 1;
                   return null;
               }
               if (pg == l2.sl.Last.Data.GetPd(l2))
               {
                   //如果pg在与当前待处理面相邻的某个面上，且为头面或尾面
                   u1.InFile(u1.infopath, "k7");
                   flag = 0;
                   return l2.sl.Last.Data;
               }
               while (sn != null)
               {
                   if (sn.Next != null)
                       if (sn.Next.Data == s)
                       {
                           u1.InFile(u1.infopath, "s21");
                           pt = sn.Data.GetPd(l2);//取得面sn.Data上除lk外的顶点
                           if (pt == pg)
                           //如果当前pt和pg是同一个点，则pg是有效点
                           {
                               if (l2 == lk)//如果l2为当前处理的边，则认为pg为普通点
                               {
                                   flag = 1;
                                   return null;
                               }
                               flag = 0;
                               return sn.Data;
                           }

                       }
                   if (sn.Data == s && sn.Next != null)
                   {
                       u1.InFile(u1.infopath, "s22");
                       pt = sn.Next.Data.GetPd(l2);//取得面sn.Next.Data上除lk外的顶点
                       if (pt == pg)
                       //如果当前pt和pg是同一个点，则pg是有效点
                       {
                           if (l2 == lk)//如果l3为当前处理的边，则认为pg为普通点
                           {
                               flag = 1;
                               return null;
                           }
                           flag = 0;
                           return sn.Next.Data;
                       }
                   }
                   sn = sn.Next;
               }
               u1.InFile(u1.infopath, "k2x");
               sn = l3.sl.Head;
               //l3.ToString();
               //lk.ToString();
               if (l3 == lk)//如果l1为当前处理的边，则认为pg为普通点
               {
                   u1.InFile(u1.infopath, "l3 == lk");
                   flag = 1;
               }
               //pg.ToString();
               if (pg == l3.sl.Head.Data.GetPd(l3))
               {
                   u1.InFile(u1.infopath, "kk9");
                   //如果pg在与当前待处理面相邻的某个面上，且为头面或尾面
                   flag = 1;
                   return null;
               }
               if (pg == l3.sl.Last.Data.GetPd(l3))
               {
                   u1.InFile(u1.infopath, "k10");
                   //如果pg在与当前待处理面相邻的某个面上，且为头面或尾面
                   flag = 0;
                   return l3.sl.Last.Data;
               }
               //l3.sl.Dispaly();
               while (sn != null)
               {
                   if (sn.Next != null)
                       if (sn.Next.Data == s)
                       {
                           u1.InFile(u1.infopath, "s31");
                           pt = sn.Data.GetPd(l3);//取得面sn.Data上除lk外的顶点
                           if (pt == pg)
                           //如果当前pt和pg是同一个点，则pg是有效点
                           {
                               if (l3 == lk)//如果l3为当前处理的边，则认为pg为普通点
                               {
                                   flag = 1;
                                   return null;
                               }
                               flag = 0;
                               return sn.Data;
                           }

                       }
                   if (sn.Data == s && sn.Next != null)
                   {
                       u1.InFile(u1.infopath, "s32");
                       pt = sn.Next.Data.GetPd(l3);//取得面sn.Next.Data上除lk外的顶点
                       if (pt == pg)
                       //如果当前pt和pg是同一个点，则pg是有效点
                       {
                           if (l3 == lk)//如果l3为当前处理的边，则认为pg为普通点
                           {
                               flag = 1;
                               return null;
                           }
                           flag = 0;
                           return sn.Next.Data;
                       }
                   }
                   sn = sn.Next;
               }
               flag = 3;
               return null;
               #endregion while
           }
           /*
            * 如果当前的点未被选过，则当前的点是普通点，按常规步骤处理点
            */
           flag = 1;
           return null;
       }
       private SURFACE  Is_Fitting_Point(SURFACE s,ref int flag,LINE lk)
       {
           /*
            * s是当前正在处理的面,flag 表示当前情况
            * 检查pg和s的三个点是否在s的三条边的面链中某个面的两侧
            * 如果点pg已存在，则返回相应点
            * lk为当前处理的边
            */
           util u1=new util();
           LINE l1, l2, l3;//获取s的三条边
           PO pt;//表示可能为当前pg的点
           l1 = s.l1;
           l2 = s.l2;
           l3 = s.l3;
           //u1.InFile(u1.infopath,"k0");
           if (pg.selected)
           /*
            * 如果当前的点已被选过，则当前的点是特殊点，需要特殊处理
            */
           {
               //u1.InFile(u1.infopath, "k1");
               #region while
               Node<SURFACE> sn = l1.sl.Head;
               //l1.ToString();
               //lk.ToString();
               if (l1 == lk)//如果l1为当前处理的边，则认为pg为普通点
               {
                   //u1.InFile(u1.infopath, "l1 == lk");
                   flag = 1;
               }
               if (pg == l1.sl.Head.Data.GetPd(l1))
               {
                   //u1.InFile(u1.infopath, "k3");
                   flag = 1;
                   return null;
               }
               if (pg == l1.sl.Last.Data.GetPd(l1))
               {
                   //如果pg在与当前待处理面相邻的某个面上，且为头面或尾面
                   flag = 0;
                   return l1.sl.Last.Data;
               }
               while (sn != null)
               { 
                   if(sn.Next!=null)
                   if (sn.Next.Data == s)
                   {
                       //u1.InFile(u1.infopath, "s11");
                       pt = sn.Data.GetPd(l1);//取得面sn.Data上除lk外的顶点
                       if (pt == pg)
                       //如果当前pt和pg是同一个点，则pg是有效点
                       {
                           if (l1 == lk)//如果l1为当前处理的边，则认为pg为普通点
                           {
                               flag = 1;
                               return null;
                           }
                           flag = 0;
                           return sn.Data;
                       }

                   }
                   if (sn.Data == s&&sn.Next !=null)
                   {
                       //u1.InFile(u1.infopath, "s12");
                       pt = sn.Next.Data.GetPd(l1);//取得面sn.Next.Data上除lk外的顶点
                       if (pt == pg)
                       //如果当前pt和pg是同一个点，则pg是有效点
                       {
                           if (l1 == lk)//如果l3为当前处理的边，则认为pg为普通点
                           {
                               flag = 1;
                               return null;
                           }
                           flag = 0;
                           return sn.Next.Data;
                       }
                   }
                   sn = sn.Next;
               }
               //u1.InFile(u1.infopath, "k1x");
               sn = l2.sl.Head;
               //l2.ToString();
               //lk.ToString();
               if (l2 == lk)//如果l1为当前处理的边，则认为pg为普通点
               {
                   //u1.InFile(u1.infopath, "k5");
                   flag = 1;
               }
               if (pg == l2.sl.Head.Data.GetPd(l2))
               {
                   //u1.InFile(u1.infopath, "k6");
                   flag = 1;
                   return null;
               }
               if (pg == l2.sl.Last.Data.GetPd(l2))
               {
                   //如果pg在与当前待处理面相邻的某个面上，且为头面或尾面
                   //u1.InFile(u1.infopath, "k7");
                   flag = 0;
                   return l2.sl.Last.Data;
               }
               while (sn != null)
               {
                   if (sn.Next != null)
                   if (sn.Next.Data == s)
                   {
                       //u1.InFile(u1.infopath, "s21");
                       pt = sn.Data.GetPd(l2);//取得面sn.Data上除lk外的顶点
                       if (pt == pg)
                       //如果当前pt和pg是同一个点，则pg是有效点
                       {
                           if (l2 == lk)//如果l2为当前处理的边，则认为pg为普通点
                           {
                               flag = 1;
                               return null;
                           }
                           flag = 0;
                           return sn.Data;
                       }

                   }
                   if (sn.Data == s && sn.Next != null)
                   {
                       //u1.InFile(u1.infopath, "s22");
                       pt = sn.Next.Data.GetPd(l2);//取得面sn.Next.Data上除lk外的顶点
                       if (pt == pg)
                       //如果当前pt和pg是同一个点，则pg是有效点
                       {
                           if (l2 == lk)//如果l3为当前处理的边，则认为pg为普通点
                           {
                               flag = 1;
                               return null;
                           }
                           flag = 0;
                           return sn.Next.Data;
                       }
                   }
                   sn = sn.Next;
               }
               //u1.InFile(u1.infopath, "k2x");
               sn = l3.sl.Head;
               //l3.ToString();
               //lk.ToString();
               if (l3 == lk)//如果l1为当前处理的边，则认为pg为普通点
               {
                   //u1.InFile(u1.infopath, "l3 == lk");
                   flag = 1;
               }
               //pg.ToString();
               if (pg == l3.sl.Head.Data.GetPd(l3))
               {
                   //u1.InFile(u1.infopath, "kk9");
                   //如果pg在与当前待处理面相邻的某个面上，且为头面或尾面
                    flag = 1;
                    return null;
               }
               if (pg == l3.sl.Last.Data.GetPd(l3))
               {
                   //u1.InFile(u1.infopath, "k10");
                   //如果pg在与当前待处理面相邻的某个面上，且为头面或尾面
                   flag = 0;
                   return l3.sl.Last.Data;
               }
               //l3.sl.Dispaly();
               while (sn != null)
               {
                   if (sn.Next != null)
                   if (sn.Next.Data == s)
                   {
                       //u1.InFile(u1.infopath, "s31");
                       pt = sn.Data.GetPd(l3);//取得面sn.Data上除lk外的顶点
                       if (pt == pg)
                       //如果当前pt和pg是同一个点，则pg是有效点
                       {
                           if (l3 == lk)//如果l3为当前处理的边，则认为pg为普通点
                           {
                               flag = 1;
                               return null;
                           }
                           flag = 0;
                           return sn.Data;
                       }

                   }
                   if (sn.Data == s && sn.Next != null)
                   {
                       //u1.InFile(u1.infopath, "s32");
                       pt = sn.Next.Data.GetPd(l3);//取得面sn.Next.Data上除lk外的顶点
                       if (pt == pg)
                       //如果当前pt和pg是同一个点，则pg是有效点
                       {
                           if (l3 == lk)//如果l3为当前处理的边，则认为pg为普通点
                           {
                               flag = 1;
                               return null;
                           }
                           flag = 0;
                           return sn.Next.Data;
                       }
                   }
                   sn = sn.Next;
               }
               flag = -1;
               return null;
               #endregion while
           }
           /*
            * 如果当前的点未被选过，则当前的点是普通点，按常规步骤处理点
            */
            flag = 1;
            return null;
       }
       private bool No_Bad_Point(PO p0)
       {
           /*
            * 检查某个点是否为某次搜索过程中已找过的点，如果
            * 为已找过的点，则返回false，否则返回true
            */
           Node<PO> pn = bad_pl.Head;
           while(pn!=null)
           {
               if (pn.Data == p0)
                   return false;
               pn = pn.Next;
           }
           return true;
       }
       private bool AddSurface(SURFACE s,LINE lk,PO p_test,LINE lm)
       {
           /*
            * 根据s与lk的面链的头尾的面的关系来确定s应该放在面链的头还是尾
            * 如果该边已经封闭则返回true,否则返回false
            * lm是当前处理的边
            */

           util u1=new util();
           PO Last_P, First_P,Lk_P;
           SURFACE Last_s, First_s;
           //lk.sl.Dispaly();
           //lk.ToString();
           if (lk.sl.Head == null)
           {
               lk.sl.Insert(s);
           }
           else
           {
               Last_s = lk.sl.Last.Data;//获取面链尾部的面
               First_s = lk.sl.Head.Data;//获取面链头部的面
               Last_P = Last_s.GetPd(lk); //获取面链尾部的面除lk外的点
               First_P = First_s.GetPd(lk);//获取面链头部的面除lk外的点
               Lk_P = s.GetPd(lk);//获取面s除lk外的点
               //if (loc == 4)
               //{
               //    u1.InFile(u1.infopath, "$$$$$$$$$$$$$$$$$$$$");
               //    Lk_P.ToPrint();
               //    First_P.ToPrint();
               //    u1.InFile(u1.infopath, "$$$$$$$$$$$$$$$$$$$");
               //}

               //lk.ToString();
               //First_s.p1.ToString();
               //First_s.p2.ToString();
               //First_s.p3.ToString();
               //u1.InFile(u1.infopath, u1.Direct(First_s, pg));
               //u1.InFile(u1.infopath, u1.Direct(First_s, Last_P));
               //u1.InFile(u1.infopath, u1.d3_Cal_Bi_Angle(lk.p1, lk.p2, First_P, Last_P));
               if(lk==lm)//如果当前边是主边
               if (u1.Direct(First_s, pg) * u1.Direct(First_s, Last_P) < 0&&u1.d3_Cal_Bi_Angle(lk.p1,lk.p2,First_P,Last_P)>0)
               {
                   lk.non_merged = false;
                   return false;
               }
               if (Lk_P == First_P)//如果pg在首面上
               {
                   lk.non_merged = false;
                   return false;
               }

               if (p_test == Last_P)
                   lk.sl.Insert(s);
               else if (p_test == First_P)
                   lk.sl.Head_Insert(s);
               else
                   u1.InFile(u1.infopath, "no matching!!");

           }
           return true;
       }    
       private void Insert(LINE lk)
       {
          /*
           * lk：某个正在被操作的边，本函数的目的是将某个生成面加到与该
           * 边以工作面相连的边的面链合适的位置
           */
           util u1 = new util();
           int flag=99;//表明当前点面位置关系
           SURFACE slast,st;//保存l0面链尾上的面对象;st保存某个面
           slast = lk.sl.Last.Data;
           LINE lk1, lk2;//取l0面链尾面除lk外的其他两条边
           lk1 = null;
           lk2 = null;
           PO pi, pj, pd,pfd;//当前所处理的面的三个顶点
           pi = lk.p1;
           pj = lk.p2;
           pd = slast.GetPd(lk);//获取面slast上除边l0外的剩余的点
           pfd = lk.sl.Head.Data.GetPd(lk);//获取面链上首面除边l0外的剩余的点
           lk1=slast.GetLine(pi,pd);
           lk2=slast.GetLine(pj, pd);
           LINE l1, l2, l3;
           SURFACE s, s1, s2;
           st = Is_Fitting_Point_2(slast, ref flag,lk);
           if (flag == 0)//如果pg为有效点，且位于与当前面相邻的某个面上
           {
               #region flga=0
               lk2 = st.GetLine(pg);
               l1 = new LINE(pg, pi);
               l2 = st.GetLine(pj);
               l3 = st.GetLine(pd);
               s = new SURFACE(pi, pg, pj);
               s1 = new SURFACE(pi, pg, pd);
               s2 = st;
               //生成新的三条边l1,l2,l3,并加入当前线链中
               pg.ll.Insert(l1);
               pi.ll.Insert(l1);
               PAINT.GetLL(l1);
               Set_direction(s, pd);
               s.l1 = l1;
               s.l2 = lk;
               s.l3 = l3;
               PAINT.GetSL(s1);
               Set_direction(s1, pj);
               s1.l1 = l1;
               s1.l2 = lk1;
               s1.l3 = l2;
               PAINT.GetSL(s2);
               Set_direction(s2, pi);
               //将生成的面加到边的面链中
               AddSurface(s1, l1, pj, lk);
               AddSurface(s1, lk1, pj, lk);
               AddSurface(s1, l2, pj, lk);
               AddSurface(s, l1, pd, lk);
               AddSurface(s, l3, pd, lk);
               AddSurface(s, lk, pd, lk);
               bad_pl.Clear();//清空坏点链
               pg.selected = true;
               #endregion flag==0
           }
           else if (flag == 1)//如果pg为普通有效点
           {
               s = new SURFACE(pi, pj, pg);
               if (AddSurface(s, lk, pd, lk))
               //如果当前处理的边未封闭
               {
                   #region flag==10
                   l1 = new LINE(pg, pi);
                   l2 = new LINE(pg, pd);
                   l3 = new LINE(pg, pj);
                   s1 = new SURFACE(pi, pd, pg);
                   s2 = new SURFACE(pg, pj, pd);
                   //生成新的三条边l1,l2,l3,并加入工作链中
                   pg.ll.Insert(l1);
                   pg.ll.Insert(l2);
                   pg.ll.Insert(l3);
                   pi.ll.Insert(l1);
                   pd.ll.Insert(l2);
                   pj.ll.Insert(l3);
                   PAINT.GetLL(l1);
                   PAINT.GetLL(l2);
                   PAINT.GetLL(l3);
                   //生成新的三个面，并使面某个方向无效
                   Set_direction(slast, pg);
                   PAINT.GetSL(s);//将新生成的面加入到面链中去
                   Set_direction(s, pd);          //确定某个点在面的方向，并使其无效
                   s.l1 = l1;
                   s.l2 = l3;
                   s.l3 = lk;
                   PAINT.GetSL(s1);
                   Set_direction(s1, pj);
                   s1.l1 = l1;
                   s1.l2 = lk1;
                   s1.l3 = l2;
                   PAINT.GetSL(s2);
                   Set_direction(s2, pi);
                   s2.l1 = l2;
                   s2.l2 = l3;
                   s2.l3 = lk2;
                   //将生成的面加到边的面链中
                   AddSurface(s, l1, pd, lk);//将面加到l1的面链中去
                   AddSurface(s, l3, pd, lk);
                   AddSurface(s1, l1, pj, lk);
                   AddSurface(s1, lk1, pj, lk);
                   AddSurface(s1, l2, pj, lk);
                   AddSurface(s2, l3, pi, lk);
                   AddSurface(s2, lk2, pi, lk);
                   AddSurface(s2, l2, pi, lk);
               #endregion
               }
               else
               {
                   #region flag==11
                   l2 = new LINE(pfd, pd);
                   s1 = new SURFACE(pi, pd, pfd);
                   s2 = new SURFACE(pfd, pj, pd);
                   //生成新的三条边l1,l2,l3,并加入工作链中
                   pfd.ll.Insert(l2);//l2 = new LINE(pg,pd);
                   pd.ll.Insert(l2);
                   PAINT.GetLL(l2);
                   l1 = lk.sl.Head.Data.GetLine(pj);
                   l3 = lk.sl.Head.Data.GetLine(pi);
                   s = lk.sl.Head.Data;
                   Set_direction(s, pd);
                   PAINT.GetSL(s1);
                   Set_direction(s1, pj);
                   s1.l1 = l1;
                   s1.l2 = lk1;
                   s1.l3 = l2;
                   PAINT.GetSL(s2);
                   Set_direction(s2, pi);
                   s2.l1 = l2;
                   s2.l2 = l3;
                   s2.l3 = lk2;
                   //将生成的面加到边的面链中
                   AddSurface(s1, l1, pj, lk);
                   AddSurface(s1, lk1, pj, lk);
                   AddSurface(s1, l2, pj, lk);
                   AddSurface(s2, l3, pi, lk);
                   AddSurface(s2, lk2, pi, lk);
                   AddSurface(s2, l2, pi, lk);
                   #endregion flag==11
               }
               bad_pl.Clear();//清空坏点链
               pg.selected = true;
           }
           else if(flag==3){//pg已存在，且在不与当前面相邻的面上
               u1.InFile(u1.infopath ,"-------------???______________");
               #region flag==10
               l1 = pg.IsLineExist(pg, pi);
               if (l1 == null)
               {
                   l1 = new LINE(pg, pi);//如果当前对象为空，则说明该线不存在
                   pg.ll.Insert(l1);//把当前生成的线加到pg的线链中去
                   pi.ll.Insert(l1);//把当前生成的线加到pi的线链中去
                   PAINT.GetLL(l1);
               }
               l2 = pg.IsLineExist(pg, pd);
               if (l2 == null)
               {
                   l2 = new LINE(pg, pd);
                   pg.ll.Insert(l2);
                   pd.ll.Insert(l2);
                   PAINT.GetLL(l2);
               }
               l3 = pg.IsLineExist(pg, pj);
               if (l3 == null)
               {
                   l3 = new LINE(pg, pj);
                   pg.ll.Insert(l3);
                   pj.ll.Insert(l3);
                   PAINT.GetLL(l3);
               }
               s = new SURFACE(pi, pj, pg);
               s1 = new SURFACE(pi, pd, pg);
               s2 = new SURFACE(pg, pj, pd);
               //生成新的三个面，并使面某个方向无效
               Set_direction(slast, pg);
               PAINT.GetSL(s);//将新生成的面加入到面链中去
               Set_direction(s, pd);          //确定某个点在面的方向，并使其无效
               s.l1 = l1;
               s.l2 = l3;
               s.l3 = lk;
               PAINT.GetSL(s1);
               Set_direction(s1, pj);
               s1.l1 = l1;
               s1.l2 = lk1;
               s1.l3 = l2;
               PAINT.GetSL(s2);
               Set_direction(s2, pi);
               s2.l1 = l2;
               s2.l2 = l3;
               s2.l3 = lk2;
               //将生成的面加到边的面链中
               u1.InFile(u1.infopath ,"a1");
               AddSurface(s, lk, pd, lk);
               AddSurface(s, l1, pd, lk);//将面加到l1的面链中去
               u1.InFile(u1.infopath, "a2");
               AddSurface(s, l3, pd, lk);
               s.ToString();
               l3.ToString();
               u1.InFile(u1.infopath, "a3");
               AddSurface(s1, l1, pj, lk);
               u1.InFile(u1.infopath, "a4");
               AddSurface(s1, lk1, pj, lk);
               u1.InFile(u1.infopath, "a5");
               AddSurface(s1, l2, pj, lk);
               u1.InFile(u1.infopath, "a6");
               AddSurface(s2, l3, pi, lk);
               s2.ToString();
               u1.InFile(u1.infopath, "a7");
               AddSurface(s2, lk2, pi, lk);
               u1.InFile(u1.infopath, "a8");
               AddSurface(s2, l2, pi, lk);
               #endregion
               //bad_pl.Insert(pg);
           }
           else
               bad_pl.Insert(pg);
           
       }
       private void SearchD(SURFACE s0)
       {
           /*
            * 针对某个面，在模型的点链中搜索某个满足条件的点
            */
            pi = s0.p1;
            pj = s0.p2;
            pk = s0.p3;
            pg = null;
            Node<PO> pt=null;
            soliangle1 = 0;
            util u1=new util();
            u1.InFile(u1.infopath, "--start SearchD--");
            pt = pl.Head;
            while (pt!=null)
            {
                soliangle2 = u1.d3_Cal_Solid_Angle(pt.Data,pi,pj,pk);
                if (soliangle1 < soliangle2&&u1.noSearched(s0,pt.Data)&&No_Bad_Point(pt.Data))
                {
                    soliangle1 = soliangle2;
                    pg = pt.Data;
                }
                pt = pt.Next;
            }
            u1.InFile(u1.infopath, "--end SearchD--");
        }
       private void Search_In(LINE lk,int num)
       {
           //对边链的某个边进行处理
           int i;
            util u1=new util();
            Node<SURFACE> sn = lk.sl.Last;
            u1.InFile(u1.infopath, "---Search_In start---");
            lk.ToString();
            u1.InFile(u1.infopath, "---该线的面链---");
            lk.sl.Dispaly();
            bad_pl.Clear();
            for (i = 0; i <num;i++ )//当当前正在处理的边未封闭时
            {
                if (sn != null)
                {
                    SearchD(sn.Data);
                    if (pg == null)
                    {
                        u1.InFile(u1.infopath, "--no point finded--");
                        break;
                    }
                    else
                    {
                        u1.InFile(u1.infopath, "----Search Result----");
                        u1.InFile(u1.infopath, pg);
                        if(lk.non_merged)
                        Insert(lk);
                    }
                    if (bad_pl.Head== null)
                        sn = sn.Next;
                    else
                        break;
                }

            }
            //if (i == 0&&loc==23) MessageBox.Show("no point finded"+','+loc+','+i);
            u1.InFile(u1.infopath, "---Search_In end---");
       }
       public void SearchLine()
       {
           //Delaunay算法的主函数，遍历边链的每一条边，并分别进行处理
           util u1 = new util();
           Node<LINE> lt = pre_l.Head;
           u1.InFile(u1.infopath, "--start SearchLine--");
           int num = 40;
           if (loc == 2) num = 8;
           for (int i = 0; i <num;i++ )
           {
               if (lt == null)
                   break;
               if (lt.Data.non_merged)
                   Search_In(lt.Data ,20);
               else
               {
                   u1.InFile(u1.infopath, "-----has been merged-----");
               }
               lt = lt.Next;
           }
           u1.InFile(u1.infopath, "--SearchLine end--");
       }
       public void SearchPoint(int num)
       {
           //Delaunay算法的主函数，遍历边链的每一条边，并分别进行处理
           util u1 = new util();
           Node<PO> pt = work_pl.Head;
           u1.InFile(u1.infopath, "--start SearchPoint--");
           for (int i = 0; i < num; i++)
           {
               loc = i;
               if (pt == null)
                   break;
               u1.InFile(u1.infopath, i);
               pkey = pt.Data;
               pt.Data.ToString();//输出当前需要处理的点
               pre_l = pt.Data.ll;
               SearchLine();
               pt = pt.Next;
           }
           u1.InFile(u1.infopath, "--SearchPoint end--");
       }
    }
}
