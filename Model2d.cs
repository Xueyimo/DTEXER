using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace arcball
{
    public abstract class model2d
    {
        protected LinkList<PO> ge_p_l;//某个对象生成的点链
        public LinkList<PO> edge_pl;//某个对象的限定点集
        public LinkList<PO> initial_pl;//模块的初始点链
        public int type;//某个对象的类型：1-直井，2-断层,3-PLANE,4-水平井，5-边界
        protected LinkList<LINE> contr_l_l;//某个对象的控制面链
        public PO pi, pj;//用于确定某个模型 三个点以便以后的三角剖分
        protected OGL PAINT = null;//用于画图的对象
        public PO center;//用于某种中点
        public void GetOGL(OGL pa)
        {
            //获取具备画图功能的对象
            PAINT = pa;
        }
        public abstract void GenerateNetPo();
        public abstract void Generate_eadge();
        public LinkList<PO> Get_p_l()
        {
            return ge_p_l;
        }
        public void Set_ge_pl(LinkList<PO> pl){
            ge_p_l =pl;
        }
        public void Set_initial_pl(LinkList<PO> pl)
        {
            initial_pl= pl;
        }
        public LinkList<LINE> Get_contr_s_l()
        {
            return contr_l_l;
        }
        public bool Is_po_inMe(PO p0)
        {
            util u1 = new util();
            if (edge_pl != null)
                return u1.Is_in_polygon(edge_pl, p0);
            else
                return false;
        }
        protected float Cal_distance_po_edge(PO p1, PO p2)
        {
            //计算p1沿p1-p2方向到边界的距离
            util u1 = new util();
            PO np;
            float f1, f2;
            f1 = 0;
            f2 = 0;
            Node<PO> pn = edge_pl.Head;
            while (pn.Next != null)
            {
                //寻找与p1,p2相交的直线段
                np = u1.IsXl(pn.Data, pn.Next.Data, p1, p2, ref f1, ref f2);
                if (f1 > 0 && f1 < 1&&f2>=0&&f1<=1)
                {//将np插入到pn-pnt之间
                    return np.Me_to_po_length(p1);
                }
                pn = pn.Next;
            }
            np = u1.IsXl(pn.Data, edge_pl.Head.Data, p1, p2, ref f1, ref f2);
            if (f1 > 0 && f1 < 1 && f2 >= 0 && f1 <= 1)
            {//将np插入到pn-pnt之间
                return np.Me_to_po_length(p1);
            }
            return 0;
        }
        protected void show_edge_l()
        {
            Node<PO> pn = edge_pl.Head;
            while (pn.Next != null)
            {
                PAINT.ll_tag.Insert(new LINE(pn.Data,pn.Next.Data));
                pn = pn.Next;
            }
            PAINT.ll_tag.Insert(new LINE(pn.Data, edge_pl.Head.Data));
        }
    }
    public class well2d : model2d
    {
        float ri;//模块的内半径，最内圈网格点到模块中心的距离，应有ri>rw；
        public float ro;//模块的外半径，最外圈网格点到模块中心的距离；
        int nr;//径向分割数，圆的一条半径穿过nr个网格；
        int nsta;//角向分割数，以（x0,y0）为圆心的半径小于r0的圆穿过的nsta个网格；
        public well2d()
        {
            type = 1;
            edge_pl = new LinkList<PO>();
            center = new PO(0,0);
            center.key = 2;
            ri = 1;
            ro = 2;
            nr = 4;
            nsta = 14;
        }
        public well2d(float x, float y)
        {
            type = 1;
            edge_pl = new LinkList<PO>();
            center = new PO(0, 0);
            center.x = x;
            center.y = y;
            center.key = 2;
            ri = 1;
            ro = 2;
            nr = 4;
            nsta = 14;
        }
        public override void GenerateNetPo()
        {
            PAINT.model_special_pl.Insert(center);
            show_edge_l();
            float yita, rx;
		    int ix, isita;
		    PO ptmp;
            PAINT.GetPL(center);
            ge_p_l.Insert(center);
		    yita = (float)System.Math.Pow(ro / ri, 1.0 / (nr - 1));
		    rx = ri / yita;
		    for (ix = 0; ix < nr; ix++){
			    rx = rx*yita;
			    for (isita = 0; isita < nsta; isita++){
				    ptmp = new PO();
				    ptmp.x = (float)(center.x + rx*System.Math.Cos(2 * 3.1415926*isita / nsta));
				    ptmp.y = (float)(center.y + rx*System.Math.Sin(2 * 3.1415926*isita / nsta));
                    if (ix == 0&&initial_pl!=null)
                        initial_pl.Insert(ptmp);
                    if(Is_po_inMe(ptmp)){
                    PAINT.GetPL(ptmp);
                    ge_p_l.Insert(ptmp);
                    }
			    }
		    }
        }
        public override void Generate_eadge()
        {
            float yita, rx;
		    int  isita;
            edge_pl.Clear();
		    PO ptmp;
            rx = ro*1.2f;
		    yita = (float)System.Math.Pow(ro / ri, 1.0 / (nr - 1));
			for (isita = 0; isita < nsta; isita++){
				ptmp = new PO();
				ptmp.x = (float)(center.x + rx*System.Math.Cos(2 * 3.1415926*isita / nsta));
				ptmp.y = (float)(center.y + rx*System.Math.Sin(2 * 3.1415926*isita / nsta));
                edge_pl.Insert(ptmp);
			}
        }
    }
    public class plane2d : model2d
    {
        PO pmax, pmin;//一个包含油藏的矩形区域的控制点
        float dx;//相邻网格点x方向间距
        float dy;//相邻网格点y方向间距
        LinkList<model2d> model_l;//位于某个面中的模块构成的链
        public plane2d()
        {
            pmax = new PO(100,100,0);
            pmin = new PO(-110,-110,0);
            dx = 1;
            dy = 1;
            edge_pl = new LinkList<PO>();
            type = 3;
        }
        public  void Get_model_l(LinkList<model2d> ml)
        {
            model_l = ml;
        }
        public bool Is_po_inallmodel(PO p0)
        {
            //检查某个点是否在某个模块中
            if (model_l == null) return false;
            Node<model2d> mnt = model_l.Head;
            while (mnt != null)
            {
                if (mnt.Data.Is_po_inMe(p0))
                {
                    return true;
                }
                mnt = mnt.Next;
            }
            return false;
        }
        public override void Generate_eadge()
        {
            PO tmp1, tmp2, tmp3, tmp4;
            tmp1 = new PO(-20, 16);
            tmp1.key = 1;
            edge_pl.Clear();
            PAINT.model_special_pl.Insert(tmp1);
            edge_pl.Insert(tmp1);
            tmp2 = new PO(22, 18);
            tmp2.key = 1;
            PAINT.model_special_pl.Insert(tmp2);
            edge_pl.Insert(tmp2);
            PAINT.model_boundary_ll.Insert(new LINE(tmp1, tmp2));
            tmp3 = new PO(19, -17);
            tmp3.key = 1;
            PAINT.model_special_pl.Insert(tmp3);
            edge_pl.Insert(tmp3);
            PAINT.model_boundary_ll.Insert(new LINE(tmp2, tmp3));
            tmp4 = new PO(-20, -19);
            tmp4.key = 1;
            PAINT.model_special_pl.Insert(tmp4);
            edge_pl.Insert(tmp4);
            PAINT.model_boundary_ll.Insert(new LINE(tmp3, tmp4));
            PAINT.model_boundary_ll.Insert(new LINE(tmp4, tmp1));
        }
        public override void GenerateNetPo()
        {
            util u1=new util();
            float x, y;
            PO tmp1;
		    for (x = pmin.x; x < pmax.x; x = x + dx)
		    for (y = pmin.y; y < pmax.y; y = y + dy){
			    tmp1 = new PO();
			    tmp1.x = x;
			    tmp1.y = y;
                if (Is_po_inMe(tmp1)&&!Is_po_inallmodel(tmp1))
                {//如果tmp在plane的边界内，且不在某个模块内
                    ge_p_l.Insert(tmp1);
                    PAINT.GetPL(tmp1);
                }
		    }
        }
    }
    public class fault2d : model2d
    {
        public LINE l0;//断层的中轴线
        float dx;//线模块方向网格间距
        float dy;//网格点到线模块限定线的距离
        public bool boundary;//标识边界
        public fault2d(LINE lx)
        {
            l0 = lx;
            dx = 1;
            dy = 0.5f;
            type =2;
            lx.p1.key = 1;
            lx.p2.key = 1;
            center = new PO();
            center.x=(lx.p1.x+lx.p2.x)/2;
            center.y=(lx.p1.y+lx.p2.y)/2;
            center.z = 0.0f;
            edge_pl = new LinkList<PO>();
            boundary = false;
        }
        public override void GenerateNetPo()
        {
		    float L, li, d,sof,distance1,distance2;//sof是初始偏差值
		    PO tmp,po;
            PAINT.GetLL(l0);
            util u1 = new util();
            show_edge_l();
		    L = l0.Length();
            sof =(float)((L-System.Math.Floor(L / dx)*dx)/2);
		    for (int i = 1; i <= System.Math.Floor(L / dx)-1; i++){
			    li =(float)(sof+i*dx);
			    tmp = new PO();
                tmp.x = (l0.p1.x * (L - li) + l0.p2.x * li) / L;
                tmp.y = (l0.p1.y * (L - li) + l0.p2.y * li) / L;
                distance1= Cal_distance_po_edge(tmp, u1.po_vertical(tmp, l0.p1, l0.p2, dy));
                distance2 = Cal_distance_po_edge(tmp, u1.po_vertical(tmp, l0.p2, l0.p1, dy));
                if (distance1 > distance2)
                    distance1 = distance2;
                po = u1.po_vertical(tmp, l0.p1, l0.p2, distance1 * 1 / 2);
                ge_p_l.Insert(po);
                PAINT.GetPL(po);          
                po = u1.po_vertical(tmp, l0.p2, l0.p1, distance1 * 1 / 2);
                ge_p_l.Insert(po);
                PAINT.GetPL(po);
		    }
	    }
        public override void Generate_eadge()
        {
		    PO us, ds, ud, dd;
            util u1 = new util();
            center.x = (l0.p1.x + l0.p2.x) / 2;
            center.y = (l0.p1.y + l0.p2.y) / 2;
            center.z = 0.0f;
            edge_pl.Clear();
            us = u1.po_vertical(l0.p1,l0.p1,l0.p2,2.0f*dy);
            ds = u1.po_vertical(l0.p1, l0.p2, l0.p1, 2.0f * dy);
            ud = u1.po_vertical(l0.p2, l0.p1, l0.p2, 2.0f * dy);
            dd = u1.po_vertical(l0.p2, l0.p2, l0.p1, 2.0f * dy);
            edge_pl.Insert(us);
            edge_pl.Insert(ud);
            edge_pl.Insert(dd);
            edge_pl.Insert(ds);
	    }
    }
    public class h_well2d:model2d{
	public LINE l0;//模块的中轴线
	float ri;//模块的内半径，最内圈网格点到模块中心的距离，应有ri>rw；
	float ro;//模块的外半径，最外圈网格点到模块中心的距离；
	int nr;//径向分割数，两侧的一个半圆的一条半径穿过nr个网格，中间井段的一条垂线穿过2nr个网格；
	int nsta;//角向分割数，两侧的一个半圆上相邻两个网格对圆心的角度为2pi/nsta,应该为偶数；
	int nd;//井段分割数，井段上网格个数(包括两侧的圆心所在的网格)；
    public	h_well2d(LINE l1){
        util u1 = new util();
        edge_pl=new LinkList<PO>();
        l1.p1.key = 1;
        l1.p2.key = 1;
        l0 = l1;
		nr = 4;
		nsta = 8;
		nd = (int)(l0.Length()/1.0f);
        ri = 1;
        ro = 2;
        type = 4;

	}
    public override void GenerateNetPo()
    {
        util u1=new util();
        show_edge_l();
		float yita,rx;
		int ix, id,isita;
		PO ptmp, pc;
        PO pk;
        pc = new PO();
		pc.x = (l0.p1.x + l0.p2.x) / 2;
		pc.y = (l0.p1.y + l0.p2.y) / 2;
		yita = (float)System.Math.Pow(ro / ri, 1.0 / (nr - 1));
		for (id = 0; id <= nd - 1; id++){
			ptmp = new PO();
			ptmp.x = ((nd - 1 - id)*l0.p1.x + id*l0.p2.x) / (nd - 1);
			ptmp.y = ((nd - 1 - id)*l0.p1.y + id*l0.p2.y) / (nd - 1);
			ge_p_l.Insert(ptmp);
            PAINT.GetPL(ptmp);
		}
		for (ix = 0; ix < nr; ix++){
			rx = (float)(ri*System.Math.Pow(yita, ix));
            for (id = 1; id <nd - 1; id++)
            {
                ptmp = new PO();
                ptmp.x = ((nd - 1 - id) * l0.p1.x + id * l0.p2.x) / (nd - 1);
                ptmp.y = ((nd - 1 - id) * l0.p1.y + id * l0.p2.y) / (nd - 1);
                pk = u1.po_vertical(ptmp, l0.p1, l0.p2, rx);
                ge_p_l.Insert(pk);
                PAINT.GetPL(pk);
                pk = u1.po_vertical(ptmp, l0.p2, l0.p1, rx);
                ge_p_l.Insert(pk);
                PAINT.GetPL(pk);
            }
			for (isita = 0; isita <=nsta / 2; isita++){
				ptmp = new PO();
                ptmp.x = (float)(l0.p1.x + rx * System.Math.Cos(2 * 3.1415926 * isita / nsta + u1.CalAgl(l0.p1, l0.p2) + 3.1415926/2));
                ptmp.y = (float)(l0.p1.y + rx * System.Math.Sin(2 * 3.1415926 * isita / nsta + u1.CalAgl(l0.p1, l0.p2) + 3.1415926 / 2));
				ge_p_l.Insert(ptmp);
                PAINT.GetPL(ptmp);
			}
			for (isita = 0; isita <=nsta / 2; isita++){
				ptmp = new PO();
                ptmp.x = (float)(l0.p2.x + rx * System.Math.Cos(2 * 3.1415926 * isita / nsta + u1.CalAgl(l0.p1, l0.p2) + 3.1415926*1.5f));
                ptmp.y = (float)(l0.p2.y + rx * System.Math.Sin(2 * 3.1415926 * isita / nsta + u1.CalAgl(l0.p1, l0.p2) + 3.1415926*1.5f));
                ge_p_l.Insert(ptmp);
                PAINT.GetPL(ptmp);
			}
		}
	}
    public override void Generate_eadge()
    {
        util u1 = new util();
        float yita, rx;
        int id, isita;
        PO ptmp, pc;
        PO pk;
        edge_pl.Clear();
        pc = new PO();
        pc.x = (l0.p1.x + l0.p2.x) / 2;
        pc.y = (l0.p1.y + l0.p2.y) / 2;
        yita = (float)System.Math.Pow((ro*1.2f) / ri, 1.0 / (nr - 1));
        rx = (float)(ri * System.Math.Pow(yita, nr-1));
        for (isita = 0; isita <= nsta / 2; isita++)
        {
            ptmp = new PO();
            ptmp.x = (float)(l0.p1.x + rx * System.Math.Cos(2 * 3.1415926 * isita / nsta + u1.CalAgl(l0.p1, l0.p2) + 3.1415926 / 2));
            ptmp.y = (float)(l0.p1.y + rx * System.Math.Sin(2 * 3.1415926 * isita / nsta + u1.CalAgl(l0.p1, l0.p2) + 3.1415926 / 2));
            edge_pl.Insert(ptmp);
        }
        //for (id = 0; id <= nd - 1; id++)
        //{
        //    ptmp = new PO();
        //    ptmp.x = ((nd - 1 - id) * l0.p1.x + id * l0.p2.x) / (nd - 1);
        //    ptmp.y = ((nd - 1 - id) * l0.p1.y + id * l0.p2.y) / (nd - 1);
        //    pk = u1.po_vertical(ptmp, l0.p1, l0.p2, rx);
        //    edge_pl.Insert(pk);
        //}
        for (isita = 0; isita <= nsta / 2; isita++)
        {
            ptmp = new PO();
            ptmp.x = (float)(l0.p2.x + rx * System.Math.Cos(2 * 3.1415926 * isita / nsta + u1.CalAgl(l0.p1, l0.p2) + 3.1415926 * 1.5f));
            ptmp.y = (float)(l0.p2.y + rx * System.Math.Sin(2 * 3.1415926 * isita / nsta + u1.CalAgl(l0.p1, l0.p2) + 3.1415926 * 1.5f));
            edge_pl.Insert(ptmp);
        }
        //for (id = nd-1; id <=0; id--)
        //{
        //    ptmp = new PO();
        //    ptmp.x = ((nd - 1 - id) * l0.p1.x + id * l0.p2.x) / (nd - 1);
        //    ptmp.y = ((nd - 1 - id) * l0.p1.y + id * l0.p2.y) / (nd - 1);
        //    pk = u1.po_vertical(ptmp, l0.p2, l0.p1, rx);
        //    edge_pl.Insert(pk);
        // }
    }
    }
    public class container:model2d 
    {
        plane2d plane;//建模所在的边界
        public LinkList<model2d> model_l;
        public LinkList<model2d> model_l_boundary;
        public container()
        {
            model_l = new LinkList<model2d>();
            ge_p_l = new LinkList<PO>();
            initial_pl = new LinkList<PO>();
            model_l_boundary = new LinkList<model2d>();
        }
        public LinkList<PO>  Get_ge_pl()
        {
            return ge_p_l;
        }
        //产生相应模块
        public well2d Create_Vertical_Well(PO center)
        {
            well2d well = new well2d(center.x,center.y);
            well.GetOGL(PAINT);
            well.Set_ge_pl(this.Get_ge_pl());
            //well.Set_initial_pl(this.initial_pl);
            well.Generate_eadge();
            model_l.Insert(well);
            return well;
        }
        public void Create_fault(LINE l0)
        {
            fault2d f = new fault2d(l0);
            PAINT.model_special_pl.Insert(l0.p1);
            PAINT.model_special_pl.Insert(l0.p2);
            f.GetOGL(PAINT);
            f.Set_ge_pl(this.Get_ge_pl());
            f.Generate_eadge();
            model_l.Insert(f);
        }
        public void Create_h_well2d(LINE l1)
        {
            h_well2d hw = new h_well2d(l1);
            PAINT.model_special_pl.Insert(l1.p1);
            PAINT.model_special_pl.Insert(l1.p2);
            hw.GetOGL(PAINT);
            hw.Set_ge_pl(this.Get_ge_pl());
            hw.Generate_eadge();
            model_l.Insert(hw);
        }
        #region Conflict_Deal
             // 处理两个断层之间的冲突
            private bool Is_Conflict(model2d m1,model2d m2)
            {
                //检查断层f1和f2是否存在冲突，如果存在返回true,否则返回false
                Node<PO> pnt = m1.edge_pl.Head;
                while (pnt != null)
                {
                    if (m2.Is_po_inMe(pnt.Data)) {
                        return true;
                    }
                    pnt = pnt.Next;
                }
                return false;
            }
            private void Cal_new_edge_po_insert(model2d m0,PO p1,PO p2)
            {
                util u1=new util();
                PO np;
                float f1, f2;
                f1 = 0;
                f2 = 0;
                bool flag = false;
                Node<PO> pn = m0.edge_pl.Head,pnt,pnt1;
                pnt1 = null;
                LINE lt = new LINE(p1,p2);
                //PAINT.pl_tag.Insert(m0.edge_pl.Last.Data);
                if(pn!=null)
                while(pn.Next!=null){
                    //寻找与p1,p2相交的直线段
                    np = u1.IsXl(pn.Data,pn.Next.Data,p1,p2,ref f1,ref f2);
                    if (f1 > 0 && f1 < 1&&np!=null)
                    {//将np插入到pn-pnt之间
                        pnt = new Node<PO>(np);
                        PAINT.pl_tag.Insert(np);
                        pnt.Next = pn.Next;
                        pn.Next = pnt;
                        pn = pn.Next;
                        if (pnt1 == null)
                        {
                            pnt1 = pnt;
                            //u1.InFile(u1.wherepath,"find point 1");
                            //u1.InFile(u1.wherepath, u1.Direct_2d(lt, pn.Next.Data));
                            //u1.InFile(u1.wherepath, u1.Direct_2d(lt, m0.center));
                            //PAINT.pl_tag.Insert(pn.Next.Data);
                            //PAINT.pl_tag.Insert(m0.center);
                            if (u1.Direct_2d(lt, pn.Next.Data) * u1.Direct_2d(lt, m0.center) < 0)
                            {
                                flag = true;
                                //u1.InFile(u1.wherepath, "flag=true");
                            }
                                
                        }
                        else
                        {
                            if (flag)
                            {
                                //u1.InFile(u1.wherepath, "deal 1");
                                pnt1.Next = pnt;
                                return;
                            }
                            else
                            {
                                //u1.InFile(u1.wherepath, "deal 2");
                                m0.edge_pl.Head = pnt1;
                                m0.edge_pl.Last = pnt;
                                m0.edge_pl.Last.Next = null;
                                return;
                            }
                        }
                    }
                    pn = pn.Next;
                }
                //u1.InFile(u1.wherepath, "middle");
                np = u1.IsXl(pn.Data, m0.edge_pl.Head.Data, p1, p2, ref f1, ref f2);
                if (f1 > 0 && f1 < 1&&np!=null)
                {//将np插入到pn-pnt之间
                    np.key = 2;
                    PAINT.pl_tag.Insert(np);
                    pnt = new Node<PO>(np);
                    pnt.Next = pn.Next;
                    pn.Next = pnt;
                    if (pnt1 != null)
                        if (flag)
                        {
                            //u1.InFile(u1.wherepath, "deal 1");
                            pnt1.Next = pnt;
                            return;
                        }
                        else
                        {
                            //u1.InFile(u1.wherepath, "deal 2");
                            m0.edge_pl.Head = pnt1;
                            m0.edge_pl.Last = pnt;
                            return;
                        }
                }
             }
            private void CD_po_together(fault2d f1,fault2d f2)
            {
                util u1=new util();
                PO f1s, f1d, f2s, f2d,ptm,p1t,p2t,p3t;
                f1s = f1.l0.p1;
                f1d = f1.l0.p2;
                f2s = f2.l0.p1;
                f2d = f2.l0.p2;
                //判断f1、f2的状态
                #region judge topo
                if (f1s.Me_to_po_length(f2d) == 0)
                {
                    ptm = f2d;
                    f2d = f2s;
                    f2s = ptm;
                }
                //u1.InFile(u1.infopath, "ihere11");
                if (f1d.Me_to_po_length(f2s) == 0)
                {
                    ptm = f1d;
                    f1d = f1s;
                    f1s = ptm;
                }
                //u1.InFile(u1.infopath, "ihere12");
                if (f1d.Me_to_po_length(f2d) == 0)
                {
                    ptm = f1d;
                    f1d = f1s;
                    f1s = ptm;
                    ptm = f2d;
                    f2d = f2s;
                    f2s = ptm;
                }
                //u1.InFile(u1.infopath, "ihere13");
                #endregion judge topo
                p1t = u1.Po_vec_f(f1s,u1.cal_vec(f1s,f1d).unitVector(),4);
                p2t = u1.Po_vec_f(f2s, u1.cal_vec(f2s, f2d).unitVector(), 4);
                p3t = new PO((p1t.x + p2t.x) / 2, (p1t.y + p2t.y) / 2);
                p1t.key = 1;
                p2t.key = 1;
                p3t.key = 1;
                PAINT.ll_tag.Insert(new LINE(f1s, p3t));
                PAINT.pl_tag.Insert(p1t);
                PAINT.pl_tag.Insert(p2t);
                PAINT.pl_tag.Insert(p3t);
                Cal_new_edge_po_insert(f1, f1s, p3t);
                Cal_new_edge_po_insert(f2, f2s, p3t);
            }
            private void DE_LL(fault2d f1,fault2d f2)
            {
                util u1 = new util();
                if (!Is_Conflict(f1, f2)) return;
                else
                {
                    CD_po_together(f1,f2);
                }

            }
            //处理两个直井之间的冲突
            private void CD_well_well(well2d w1,well2d w2)
            {
                util u1 = new util();
                PO mid,p1,p2;
                mid = new PO();
		        mid.x = (w1.center.x*w1.ro+ w2.center.x*w2.ro) / (w1.ro + w2.ro);
                mid.y = (w1.center.y*w1.ro+ w2.center.y*w2.ro) / (w1.ro + w2.ro);
                p1 = u1.po_vertical(mid,w1.center,w2.center,5);
                p2 = u1.po_vertical(mid,w2.center,w1.center,5);
                u1.InFile(u1.tri_num,"-------------------IN well");
                Cal_new_edge_po_insert(w1,p1,p2);
                u1.InFile(u1.tri_num, "-------------------end well1");
                Cal_new_edge_po_insert(w2,p1,p2);
                u1.InFile(u1.tri_num, "-------------------end well");
            }
            private void DE_WW(well2d w1,well2d w2)
            {
                util u1 = new util();
                if (!Is_Conflict(w1, w2)) return;
                else
                {
                    CD_well_well(w1,w2);
                }
            }
            //冲突处理的总函数
            private void Conflict_Deal(model2d m1,model2d m2)
            {
                int t1, t2;
                t1 = m1.type;
                t2 = m2.type; //1-直井，2-断层,3-PLANE
                if (t1 == 2 && t2 ==2)
                    DE_LL((fault2d)m1, (fault2d)m2);
                if (t1 == 1 && t2 == 1)
                    DE_WW((well2d)m1,(well2d)m2);
            }
            public void DealModuleConflict(){
                /*
                 * 处理模块间的冲突，这是一个总函数，
                 * 将任意两个模块进行冲突的检测
                 */
                Node<model2d> mnt,mntn;
                mnt=model_l.Head;
		        while (mnt!=null){
			        mntn=mnt.Next;
			        while (mntn!=null){
                        Conflict_Deal(mnt.Data,mntn.Data);
				        mntn=mntn.Next;
			        }
			        mnt=mnt.Next;
		        }
	        }
        #endregion
        public void Output_model_info()
        {
            util u1 = new util();
            FileStream stream = File.Open(u1.model_info, FileMode.OpenOrCreate, FileAccess.Write);
            stream.Seek(0, SeekOrigin.Begin);
            stream.SetLength(0);
            stream.Close();//清空文件内容
            StreamWriter sw = new StreamWriter(u1.model_info, true);
            Node<model2d> mnt = model_l.Head;
            while (mnt != null)
            {
                switch (mnt.Data.type)
                {
                    case 1:
                        sw.WriteLine("1:({0},{1})", ((well2d)mnt.Data).center.x, ((well2d)mnt.Data).center.x);
                        break;
                    case 2:
                        if (!((fault2d)mnt.Data).boundary)
                        sw.WriteLine("2:({0},{1}),({2},{3})", ((fault2d)mnt.Data).l0.p1.x, ((fault2d)mnt.Data).l0.p1.y, ((fault2d)mnt.Data).l0.p2.x, ((fault2d)mnt.Data).l0.p2.y);
                        break;
                    case 4:
                        sw.WriteLine("4:({0},{1}),({2},{3})", ((h_well2d)mnt.Data).l0.p1.x, ((h_well2d)mnt.Data).l0.p1.y, ((h_well2d)mnt.Data).l0.p2.x, ((h_well2d)mnt.Data).l0.p2.y);
                        break;
                }
                mnt = mnt.Next;
            }
            Node<PO> pnt = plane.edge_pl.Head;
            while(pnt!=null)
            {
                sw.WriteLine("0:({0},{1})", pnt.Data.x, pnt.Data.y);
                pnt = pnt.Next;
            }
            sw.Close();
        }
        public void In_model()
        {
            model_l.Clear();
            plane.edge_pl.Clear();
            util u1 = new util();
            string sn = "";
            string str;
            float[] fn = new float[9];
            int j = 0;
            int first = 0;
            well2d w0;
            StreamReader sr = new StreamReader(u1.model_info, true);
            while ((str = sr.ReadLine()) != null)
            {
                j = 0;
                foreach (char ch in str)
                {
                    if ((int)ch >= 48 && (int)ch <= 57 || ch.Equals('.') || ch.Equals('-') || ch.Equals('E'))
                        sn = sn + ch;
                    else
                    {
                        if (!sn.Equals(""))
                            fn[j++] = float.Parse(sn);
                        sn = "";
                    }
                }
                
                //1,2,3,4,5,6
                PO[] pc = new PO[2];
                pc[0] = new PO();
                pc[0].x = fn[1];
                pc[0].y = fn[2];
                pc[0].z = 0;
                pc[1] = new PO();
                pc[1].x = fn[3];
                pc[1].y = fn[4];
                pc[1].z = 0;
                switch ((int)fn[0])
                {
                    case 1:
                        u1.InFile(u1.wherepath, ("well:"+pc[0].x + ',' + pc[0].y + ',' + pc[0].z));
                        w0=Create_Vertical_Well(pc[0]);
                        if(first++==0)
                        {
                            center = w0.center;
                            w0.Set_initial_pl(initial_pl);
                        }
                        break;
                    case 2:
                        Create_fault(new LINE(pc[0],pc[1]));
                        u1.InFile(u1.wherepath, ("fault:" + pc[0].x + ',' + pc[0].y + ',' + pc[0].z +','+ pc[1].x + ',' + pc[1].y + ',' + pc[1].z));
                        break;
                    case 4:
                        Create_h_well2d(new LINE(pc[0],pc[1]));
                        u1.InFile(u1.wherepath, ("h_well:" + pc[0].x + ',' + pc[0].y + ',' + pc[0].z + ',' + pc[1].x + ',' + pc[1].y + ',' + pc[1].z));
                        break;
                    case 0:
                        plane.edge_pl.Insert(pc[0]);
                        break;
                }
            }
        }
        public void Generate_allmodel_edge_po()
        {
            Node<model2d> mnt = model_l.Head;
            while (mnt != null)
            {
                switch (mnt.Data.type)
                {
                    case 1:
                        ((well2d)mnt.Data).Generate_eadge();
                        break;
                    case 2:
                        ((fault2d)mnt.Data).Generate_eadge();
                        break;
                    case 4:
                        ((h_well2d)mnt.Data).Generate_eadge();
                        break;
                }
                mnt = mnt.Next;
            }
        }
        public void Generate_allmodel_po()
        {
            Node<model2d> mnt = model_l.Head;
            while (mnt != null)
            {
                switch (mnt.Data.type)
                {
                    case 1:
                        ((well2d)mnt.Data).GenerateNetPo();
                        break;
                    case 2:
                        ((fault2d)mnt.Data).GenerateNetPo();
                        break;
                    case 4:
                        ((h_well2d)mnt.Data).GenerateNetPo();
                        break;
                }
                mnt = mnt.Next;
            }
        }
        private void Create_boundary_model()
        {
            util u1 = new util();
            Node<PO> pnt = plane.edge_pl.Head;
            fault2d f;
            while (pnt.Next != null)
            {
                f = new fault2d(new LINE(pnt.Data,pnt.Next.Data));
                f.boundary = true;
                f.GetOGL(this.PAINT);
                f.Set_ge_pl(this.ge_p_l);
                f.Generate_eadge();
                model_l.Insert(f);
                pnt = pnt.Next;
            }
            f = new fault2d(new LINE(pnt.Data, plane.edge_pl.Head.Data));
            f.boundary = true;
            f.GetOGL(this.PAINT);
            f.Set_ge_pl(this.ge_p_l);
            f.Generate_eadge();
            model_l.Insert(f);
        }
        private void Clear_boundary_model()
        {
            Node<model2d> mnt = model_l.Head;
            Node<model2d> mnt2=null;
            while (mnt != null)
            {
                if(mnt.Data.type==2)
                {
                    if(((fault2d)mnt.Data).boundary)
                    if (model_l.Head == mnt)
                    {
                        model_l.Head= model_l.Head.Next;
                        mnt = model_l.Head;
                        continue;
                    }
                    else
                    {
                        mnt2.Next= mnt.Next;
                        mnt = mnt2;
                    }
                    
                }
                mnt2=mnt;
                mnt = mnt.Next;
            }
            model_l.Last = mnt2;
        }
        public void START()
        {
            util u1 = new util();
            well2d w1 = new well2d();
            w1.GetOGL(PAINT);
            w1.Set_ge_pl(ge_p_l);
            w1.Set_initial_pl(initial_pl);
            center = w1.center;
            model_l.Insert(w1);
            plane = new plane2d();
            plane.GetOGL(PAINT);
            plane.Set_ge_pl(ge_p_l);
            plane.Generate_eadge();
            Create_boundary_model();
            Generate_allmodel_edge_po();
            DealModuleConflict();
            plane.Get_model_l(model_l);//把模块链给plane
            plane.GenerateNetPo();//plane根据自己的边界和模块产生点
            Generate_allmodel_po();//产生所有模块的点
        }
        public void START_form(OGL ogl)
        {
            util u1 = new util();
            //清空模型中产生的所有数据，并恢复到START运行前的状态；
            Clear_boundary_model();
            PAINT = ogl;
            change_OGL();
            ge_p_l.Clear();//清空模型的生成点链；
            initial_pl.Clear();//清空初始点链
            //保证结构参数不变
            Create_boundary_model();
            Generate_allmodel_edge_po();
            DealModuleConflict();
            plane.Get_model_l(model_l);//把模块链给plane
            plane.GenerateNetPo();//plane根据自己的边界和模块产生点
            Generate_allmodel_po();//产生所有模块的点
            u1.InFile(u1.wherepath,ge_p_l.num);
        }
        public void START_Inmodel(OGL ogl)
        {
            util u1 = new util();
            //清空模型中产生的所有数据，并恢复到START运行前的状态；
            In_model();
            PAINT = ogl;
            change_OGL();
            ge_p_l.Clear();//清空模型的生成点链；
            initial_pl.Clear();//清空初始点链
            //保证结构参数不变
            Create_boundary_model();
            Generate_allmodel_edge_po();
            DealModuleConflict();
            plane.Get_model_l(model_l);//把模块链给plane
            plane.GenerateNetPo();//plane根据自己的边界和模块产生点
            Generate_allmodel_po();//产生所有模块的点
            u1.InFile(u1.wherepath, ge_p_l.num);
        }
        public void change_OGL()
        {
            plane.GetOGL(PAINT);
            Node<model2d> mnt;
            mnt = model_l.Head;
            while (mnt != null)
            {
                mnt.Data.GetOGL(PAINT);
                if (mnt.Data.center != null)
                {
                    mnt.Data.center.selected = false;
                    mnt.Data.center.key = 2;
                    mnt.Data.center.ll.Clear();
                    mnt.Data.center.non_merged = true;
                }
                mnt = mnt.Next;
            }
        }
        public override void GenerateNetPo()
        {
            return;
        }
        public override void Generate_eadge()
        {
            throw new NotImplementedException();
        }
    }
}
