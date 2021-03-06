﻿using System;
using System.Collections.Generic;
using System.Text;


namespace arcball
{
    class Mode
    {
    }
    public class PO
    {
        /**
         * 在二维的情况下，只用其前两个坐标属性值，
         * 在三维的情况下，使用三个坐标属性值
        **/
        public float x;
        public float y;
        public float z;
        public LinkList<SURFACE> sl;//表示当前的点的面链
        public LinkList<LINE> ll;//表示当前的点的线链
        public bool non_merged;//true表示当前的点的线链已经未封闭，false表示当前的点的线链封闭
        public bool selected;//selected=true表示当前的点被选过
        public PO p_parent;//p_parent=null表示当前点不是影子点，并且无父点
        public LinkList<PO> shadow_pl;//某个点的影子点链 
        public int number;//表示该点的序号
        public int key;//表示该点是否是井所在的点:0-表示普通点；1-表示边界点；2-表示井位;3-表示初始点;4-表示
        //如果一个点是井位，则在进行点的搜索时不考虑对它的搜索

        public PO()
        {
            x = 0.0f;
            y = 0.0f;
            z = 0.0f;
            key = 0;//所有点产生时都是普通点
            sl=new LinkList<SURFACE>();
            ll = new LinkList<LINE>();
            selected = false;
            non_merged = true;
        }
        public PO(float x1, float y1, float z1)
        {
            //用于三维的情况
            x = x1;
            y = y1;
            z = z1;
            key = 0;//所有点产生时都是普通点
            sl = new LinkList<SURFACE>();
            ll = new LinkList<LINE>();
            selected = false;
            non_merged = true;
        }
        public PO(float x1, float y1)
        {
            //用于二维的情况
            x = x1;
            y = y1;
            z = 0.0f;
            key = 0;//所有点产生时都是普通点
            sl = new LinkList<SURFACE>();
            ll=new LinkList<LINE>();
            selected = false;
            non_merged = true;
        }
        public void ToPrint()
        {
            util u1 = new util();
            u1.InFile(u1.infopath, this);
        }
        public override string  ToString()
        {
            util u1 = new util();
            u1.InFile(u1.infopath, this);
            return ":"+ '('+x+','+y+','+z+')';
        }
        public void ToPrint(LinkList<String> infost)
        {
            infost.Insert(":" + '(' + x + ',' + y + ',' + z + ')');
        }
        //the functions for calculating Solid Angle
        public virtual float length()
        {
            return (float)System.Math.Sqrt(x * x + y * y + z * z);
        }
        public PO unitVector()
        {
            float len = this.length();
            if (this.length() != 0)
            {
                x = x / len;
                y = y / len;
                z = z / len;
            }
            return this;
        }
        public bool IsEqualToMe(PO pi)
        {
            /*
             * 判断某个点是否和自己是同一个点,如果是返回true,如果否返回false
             */
            if (pi.x == x && pi.y == y && pi.z == z)
                return true;
            else
                return false;
        }
        public LINE IsLineExist(PO p1,PO p2)
        {
            /*
             * 检测某个点的线链中是否存在某条线
             * 如果存在就返回该先对象，
             * 否则返回null
             */
            Node<LINE> ln = new Node<LINE>();
            ln = ll.Head;
            while (ln != null)
            {
                if (ln.Data.p1 == p1 && ln.Data.p2 == p2)
                    return ln.Data;
                if (ln.Data.p1 == p2 && ln.Data.p2 == p1)
                    return ln.Data;
                ln = ln.Next;
            }
            return null;
        }
        public float Me_to_po_length(PO p0)
        {
            //返回该点到p0的距离
            util u1=new util();
            PO pt=u1.cal_vec(this,p0);
            pt.z=0;
            return pt.length();
        }
        public void Clear()
        {
            ll.Clear();
            non_merged=true;
            selected=false;
            p_parent = null;
        }
    }
    public class SURFACE
    {
        public PO p1,p2,p3;//面的三个顶点，相互对等
        public LINE l1, l2, l3;//面的三个边，相互对等
        public bool positive;//表示面的正面是否已搜索,true未搜索，false已搜索
        public bool negtive;//表示面的负面是否已搜索，true未搜索，false已搜索
        public SURFACE(PO pi, PO pj, PO pk)
        {
            p1 = pi;
            p2 = pj;
            p3 = pk;
            l1 = new LINE(p1,p2);
            l2 = new LINE(p1,p3);
            l3 = new LINE(p2,p3);
            positive = true;
            negtive = true;
        }
        public void ToPrint()
        {
            util u1 = new util();
            u1.InFile(u1.infopath, "---surface---");
            p1.ToPrint();
            p2.ToPrint();
            p3.ToPrint();
            u1.InFile(u1.infopath, "---end---");
        }
        public override string ToString()
        {
            util u1 = new util();
            u1.InFile(u1.infopath, "---surface---");
            p1.ToPrint();
            p2.ToPrint();
            p3.ToPrint();
            u1.InFile(u1.infopath, "---end---");
            return "ok";
        }
        public PO GetPd(LINE l0)
        {
            /*
             * 获取一个面中除去某条边外的另一个点
             */
            if (p1.IsEqualToMe(l0.p1) && p2.IsEqualToMe(l0.p2))
                return p3;
            if(p1.IsEqualToMe(l0.p2) && p2.IsEqualToMe(l0.p1))
                return p3;
            if (p3.IsEqualToMe(l0.p1) && p2.IsEqualToMe(l0.p2))
                return p1;
            if (p3.IsEqualToMe(l0.p2) && p2.IsEqualToMe(l0.p1))
                return p1;
            if (p1.IsEqualToMe(l0.p1) && p3.IsEqualToMe(l0.p2))
                return p2;
            if (p1.IsEqualToMe(l0.p2) && p3.IsEqualToMe(l0.p1))
                return p2;
            return null;
        }
        public LINE GetLine(PO pt0,PO pt1)
        {
            //根据两个点来决定面上与之对应的线
            if (pt0 == l1.p1 && pt1 == l1.p2)
                return l1;
            if (pt0 == l1.p2 && pt1 == l1.p1)
                return l1;
            if (pt0 == l2.p1 && pt1 == l2.p2)
                return l2;
            if (pt0 == l2.p2 && pt1 == l2.p1)
                return l2;
            if (pt0 == l3.p1 && pt1 == l3.p2)
                return l3;
            if (pt0 == l3.p2 && pt1 == l3.p1)
                return l3;
            return null;
        }
        public LINE GetLine(PO ptx)
        {
            if (ptx.x == p1.x && ptx.y == p1.y && ptx.z == p1.z)
                return GetLine(p2,p3);
            if (ptx.x == p2.x && ptx.y == p2.y && ptx.z == p2.z)
                return GetLine(p3, p1);
            if (ptx.x == p3.x && ptx.y == p3.y && ptx.z == p3.z)
                return GetLine(p2, p1);
            return null;
        }
    }
    public class LINE
    {
        public PO p1;//线对象的第一个点
        public PO p2;//线对象的第二个点
        public LinkList<SURFACE> sl;//表示当前边的面链
        public bool non_merged;//true表示当前的边的面链已经未封闭，false表示当前的边的面链封闭
        /**dedicated for 2D**/
        public bool positive;//表示线的正面是否已搜索,true未搜索，false已搜索
        public bool negtive;//表示线的负面是否已搜索，true未搜索，false已搜索
        public bool chosed;//表示当前点是否被搜索，true表示已被搜索，false表示未被搜索
        public LINE(PO pi,PO pj)
        {
            p1 = pi;
            p2 = pj;
            non_merged = true;
            sl = new LinkList<SURFACE>();
            positive = true;
            negtive = true;
            chosed = false;
        }
        public override string ToString()
        {
            util u1 = new util();
            string str="ok";
            u1.InFile(u1.infopath, "---LINE---");
            p1.ToPrint();
            p2.ToPrint();
            u1.InFile(u1.infopath, "---end---");
            return ":"+'('+p1.x+','+p1.y+','+p1.z+')'+'('+p2.x+','+p2.y+','+p2.z+')';
        }
        public void ToPrint(LinkList<String> infost)
        {
            infost.Insert("---LINE---");
            p1.ToPrint(infost);
            p2.ToPrint(infost);
            infost.Insert("---end---");
        }
        public void ToPrint()
        {
            util u1 = new util();
            u1.InFile(u1.infopath, "---LINE---");
            p1.ToPrint();
            p2.ToPrint();
            u1.InFile(u1.infopath, "---end---");
        }
        public bool Equal_to_me(LINE l0){
            return true;
        }
        public bool Po_insideme(PO p0)
        {
            //如果p0是该线的端点，则返回true，否则返回false
            if (p0.IsEqualToMe(p1) || p0.IsEqualToMe(p2))
                return true;
            else
                return false;
        }
        public SURFACE Get_Surface(SURFACE s0)
        {
            /*
             * 返回在当前线的面链中与当前面s0相邻的面，而且该面在面s0的合适位置
             */
            util u1=new util();
            Node<SURFACE> sn = sl.Head;
            PO pt;//用于记录需要检测的面上的一个点
            //如果s0是sl的第一个面，则返回sl的最后一个面
            if (sn.Data == s0&&sl.Head.Next!=null)
            {
                pt = sl.Last.Data.GetPd(this);
                if (u1.noSearched(sn.Data, pt))
                    return sl.Last.Data; 
            }
            //如果s0是最后一个面,返回第一个面
            if (sl.Last.Data == s0&&sl.Head!=sl.Last)
            {
                pt = sl.Head.Data.GetPd(this);
                if (u1.noSearched(sl.Last.Data, pt))
                    return sl.Head.Data;
            }
            //如果s0既不是第一个面也不是最后一个面
            while (sn != null)
            {
                //考查s0的后一个面
                if(sn.Data==s0&&sn.Next !=null)
                {
                    pt=sn.Next.Data.GetPd(this);
                    if (u1.noSearched(sn.Data , pt))
                        return sn.Next.Data;                    
                }
                //考查s0的前一个面
                if(sn.Next!=null&&sn.Next.Data==s0)
                {
                    pt = sn.Data.GetPd(this);
                    if (u1.noSearched(sn.Next.Data, pt))
                        return sn.Data;
                }
                sn = sn.Next;
            }
            //如果sl中不存在s0面
            return null;
        }
        public void Po_insideme_and_changeit(PO p0)
        {
            //如果p0是该线的端点，则用它交换对应端点
            if (p0.x == p1.x && p0.y == p1.y && p0.z == p1.z)
                p1 = p0;
            if (p0.x == p2.x && p0.y == p2.y && p0.z == p2.z)
                p2 = p0;
                
        }
        public PO getanotherpo(PO p0){
        //返回线的两个端点中除p0的另一个点
            if (p1.IsEqualToMe(p0))
                return p2;
            else
                return p1;
        }
        public float Length()
        {
            PO vec;
            util u1 = new util();
            vec = u1.cal_vec(p1,p2);
            return vec.length();
        }
    }
    public class TriAngle
    {
        public int number;//当前三角形的序号
        public int pn1, pn2, pn3;//当前的三角形的三个顶点的序号
        public TriAngle(int number0, int pn10, int pn20, int pn30)
        {
            number = number0;
            pn1 = pn10;
            pn2 = pn20;
            pn3 = pn30;
        }
    }
}
