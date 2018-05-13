using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace arcball
{
    public class Delaunay_2D
    {
        #region definition
        private float soliangle1, soliangle2;//用于选择角最大的点
        private LinkList<PO> pl;//表示当前所处理的点集
        public int loc = 0;//表示当前搜索到那个点
        protected OGL PAINT = null;//用于画图的对象
        private PO pkey;//表示当前正在处理的点
        private PO pg;//代表最后选择的点
        private LinkList<LINE> pre_l;//表示当前正在被处理的线链
        private Node<LINE> nt_current;//表示当前正在被处理的边节点
        public LinkList<PO> work_pl;//表示当前工作的点链集
        private LinkList<PO> initial_pl;//表示当前工作的点链集
        private LinkList<LINE> initial_ll;//通过初始的点集得到的初始边集
        private PO initial_center;//表示确定当前工作的点链集方向的点
        int number;//表示点的序号
        int numbertri;//表示三角形的序号
        int initial_side_num;//表示最初的边的数量
        //数据网格的相关数据结构
        float Xi, Yi;//表示网格系统在x和y方向的疏密程度
        int Mx, Ny;//表示网格系统在x和y方向的网格数
        int X, Y;//表示实际点与网格系统的原点间的偏差
        LinkList<PO> net_choosed;//用于所选取的网格的相关参数，所有数据为整型
        LinkList<PO>[,] DataNet;//存放要搜索的点的数据结构
        private Node<PO> Where_pointer;//指示当前执行到那个点了
        public bool STOP=false;//停止主函数
        public LinkList<String> info;//存放输出信息
        #endregion
        public Delaunay_2D()
        {
            //初始化工作边集
            number = 0;
            numbertri = 0;
            Xi = 1;
            Yi = 1;
            X = 200;
            Y = 200;
            Mx = 500;
            Ny = 500;
            work_pl = new LinkList<PO>();
            net_choosed = new LinkList<PO>();
            initial_ll = new LinkList<LINE>();
            DataNet=new LinkList<PO>[Mx,Ny];
            info = new LinkList<string>();
        }
        #region 与其他类的接口
        public void GetOGL(OGL pa)
        {
            //获取具备画图功能的对象
            PAINT = pa;
        }
        public void GetPL(LinkList<PO> pl0)
        {
            pl = pl0;
        }
        public void Get_initialpl(LinkList<PO> pl0)
        {
            initial_pl = pl0;
        }
        public void Get_initial_center(PO center0)
        {
            initial_center = center0;
        }
        #endregion 
        #region Data_Net
        private PO[] Look_for_four_PO(PO p10, PO p20, float d){
	        /*
             * 输入：要处理的边的两个顶点，两个圆弧的圆心到要处理的边的距离；
             * 输出：一个长方形的四个顶点，且它们在集合中有序存放；        
	         */
	        PO pm, vt, vt0, vt10, pm0,p1,p2;
            p1 = new PO();
            p2 = new PO();
            p1.x = p10.x + X;
            p1.y = p10.y + Y;
            p2.x = p20.x + X;
            p2.y = p20.y + Y;//将实际点变换到网格系统中的点
            PO[] p0=new PO[4];
            for (int i = 0; i < 4; i++)
                p0[i] = new PO();
	        float d0, d1, r;
            pm=new PO();
            vt=new PO();
            vt0=new PO();
            vt10=new PO();
            pm0=new PO();
	        d0 = (float)(System.Math.Pow((p1.x - p2.x)*(p1.x - p2.x) + (p1.y - p2.y)*(p1.y - p2.y), 0.5));//计算当前要处理的边的长度
	        r = (float)(System.Math.Pow(System.Math.Pow(d, 2) + System.Math.Pow(d0 / 2, 2), 0.5));//计算当前边所对应的圆的半径的长度
	        pm.x = (p1.x + p2.x) / 2;
	        pm.y = (p1.y + p2.y) / 2;//计算当前边的中点的坐标
	        vt.x = p1.x - p2.x;
	        vt.y = p1.y - p2.y;
	        d1 = (float)(System.Math.Pow(vt.x*vt.x + vt.y*vt.y, 0.5));
	        vt.x = vt.x / d1;
	        vt.y = vt.y / d1;//计算沿当前边的向量对应的单位向量
	        vt0.x = -vt.y;
	        vt0.y = vt.x;//计算与当前边垂直的向量对应的单位向量
	        vt10.x = pm.x + vt0.x*(r + d);
	        vt10.y = pm.y + vt0.y*(r + d);
	        p0[0].x = vt10.x + vt.x*r;
	        p0[0].y = vt10.y + vt.y*r;//计算长方形的第一个角点的坐标
	        p0[1].x = vt10.x - vt.x*r;
	        p0[1].y = vt10.y - vt.y*r;//计算长方形第二个角点的坐标
	        vt10.x = pm.x - vt0.x*(r + d);
	        vt10.y = pm.y - vt0.y*(r + d);
	        p0[2].x = vt10.x + vt.x*r;
	        p0[2].y = vt10.y + vt.y*r;//计算长方形的第一个角点的坐标
	        p0[3].x = vt10.x - vt.x*r;
	        p0[3].y = vt10.y - vt.y*r;//计算长方形第二个角点的坐标
	        for (int i = 0; i <= 2; i++)
	        for (int j = i + 1; j <= 3; j++)
	        if (p0[i].y > p0[j].y){
		        pm0.x = p0[i].x;
		        pm0.y = p0[i].y;
		        p0[i].x = p0[j].x;
		        p0[i].y = p0[j].y;
		        p0[j].x = pm0.x;
		        p0[j].y = pm0.y;
	        }
            return p0;
        }
	    void Four_po_netnumber(PO[] p){
            /*
             * 输入：长方形区域的四个角点的坐标
             * 输出：长方形所包括的网格的坐标
             */
		    util u1=new util();
		    int x1, x1g, x2, x2g, xm, X0, Y0, X1, Y1, i;
		    PO p1d, p2d, pm;
            pm = new PO();
		    if (u1.Direct_2d(new LINE(p[1],p[2]),p[3])*u1.Direct_2d(new LINE(p[1],p[2]),p[0]) >= 0){
			    pm.x = p[2].x;
			    pm.y = p[2].y;
			    p[2].x = p[3].x;
			    p[2].y = p[3].y;
			    p[3].x = pm.x;
			    p[3].y = pm.y;
		    }
		    X0 = (int)(p[0].x / Xi);
		    Y0 = (int)(p[0].y / Yi);
		    X1 = (int)(p[3].x / Xi);
		    Y1 = (int)(p[3].y / Yi);
		    p1d = p[0];
		    p2d = p[0];
		    x1g = x2g = 0;
            net_choosed.Clear();
		    for (i = Y0 + 1; i <= Y1 + 1; i++){
			    if (p1d.x == p[1].x) x1 = (int)(p[1].x / Xi);
			    else if (p1d.y == p[1].y) x1 =(int)(p[1].x / Xi);
			    else{
				    x1 =(int)( u1.Cal_po_on_Line(p1d, p[1], (i + x1g)*Yi) / Xi);
			    }
			    if (p2d.x == p[2].x) x2 = (int)(p[2].x / Xi);
			    else if (p2d.y == p[2].y) x2 =(int)(p[2].x / Xi);
			    else{
				    x2 =(int)( u1.Cal_po_on_Line(p2d, p[2], (i + x2g)*Yi) / Xi);
			    }
			    if (i - 1 == (int)(p[1].y / Yi)){
				    x1 = (int)(p[1].x / Xi);
				    p1d = p[3];
				    x1g = -1;
			    }
			    if (i - 1 == (int)(p[2].y / Yi)){
				    x2 = (int)(p[2].x / Xi);
				    x2g = -1;
				    p2d = p[3];
			    }
			    if (x1 > x2) { xm = x1; x1 = x2; x2 = xm; }
			    while (x1 <= x2){
                    net_choosed.Insert(new PO(x1,i-1));
				    x1++;
			    }
		    }
	    }
	    void Two_po_netnumberlist(LINE l0, float d){
		    /*
		    输入：要处理的边的两个顶点，两个圆弧的圆心到要处理边的距离；
		    输出：对应到数据网格中的方块的坐标组成的链表；
		    */
		    PO[] p;
            p = Look_for_four_PO(l0.p1, l0.p2, d);
            Four_po_netnumber(p);
	    }
        void DataNet_initial()
        {
            for(int i=0;i<Mx;i++)
                for(int j=0;j<Ny;j++)
                {
                    DataNet[i, j] = new LinkList<PO>();
                }
        }
        void Into_DataNet()
        {
            /*
             * 将点链中的点倒入到网格系统中
             */
            util u1 = new util();
		    int x0, y0;
            //u1.InFile(u1.wherepath,"here1x");

            DataNet_initial();
            Node<PO> ptn=pl.Head;
		    while (ptn != null){
			    //进行坐标变换，坐标原点是（-400,-400）
			    x0 = (int)((ptn.Data.x+X) / Xi);
			    y0 = (int)((ptn.Data.y+Y) / Yi);
                //u1.InFile(u1.wherepath, "here2x");
			    DataNet[x0,y0].Insert(ptn.Data);
                //u1.InFile(u1.wherepath, "here3x");
			    ptn = ptn.Next;
		    }
        }
        #endregion
        private void PEBIGrid(PO p0){
        util u1 = new util();
		Node<LINE> la,lb,lc;
		PO pk1,pk2,pk3;
        PO p1, p2;
		la =p0.ll.Head;
		while (la.Next.Next!=null){
			lb = la.Next;
			lc = la.Next.Next;
			if (la.Data.p1==p0) pk1 = la.Data.p2;
            else pk1 = la.Data.p1;
            if (lb.Data.p1 == p0) pk2 = lb.Data.p2;
            else pk2 = lb.Data.p1;
            if (lc.Data.p1 == p0) pk3 = lc.Data.p2;
            else pk3 = lc.Data.p1;
			p1 = u1.SearchOutHeart(p0, pk1, pk2);
			p2 = u1.SearchOutHeart(p0, pk2, pk3);
            PAINT.GetLL_pebi(new LINE(p1,p2));
			la = la.Next;
		}
        PO pt1, pt2;
        pt1 = p0.ll.Head.Data.getanotherpo(p0);
        pt2 = p0.ll.Last.Data.getanotherpo(p0);
        if (Is_OutSide(pt1,pt2)!=null)
        {
            lb = la.Next;
            lc = p0.ll.Head;
            if (la.Data.p1.IsEqualToMe(p0)) pk1 = la.Data.p2;
            else pk1 = la.Data.p1;
            if (lb.Data.p1.IsEqualToMe(p0)) pk2 = lb.Data.p2;
            else pk2 = lb.Data.p1;
            if (lc.Data.p1.IsEqualToMe(p0)) pk3 = lc.Data.p2;
            else pk3 = lc.Data.p1;
            p1 = u1.SearchOutHeart(p0, pk1, pk2);
            p2 = u1.SearchOutHeart(p0, pk2, pk3);
            PAINT.GetLL_pebi(new LINE(p1, p2));
            //l2
            la = p0.ll.Last;
            lb = p0.ll.Head;
            lc = lb.Next;
            if (la.Data.p1.IsEqualToMe(p0)) pk1 = la.Data.p2;
            else pk1 = la.Data.p1;
            if (lb.Data.p1.IsEqualToMe(p0)) pk2 = lb.Data.p2;
            else pk2 = lb.Data.p1;
            if (lc.Data.p1.IsEqualToMe(p0)) pk3 = lc.Data.p2;
            else pk3 = lc.Data.p1;
            p1 = u1.SearchOutHeart(p0, pk1, pk2);
            p2 = u1.SearchOutHeart(p0, pk2, pk3);
            PAINT.GetLL_pebi(new LINE(p1, p2));
        }
	}
        private LINE Is_OutSide(PO p0,PO ptest)
        {
            /*
             * 检测ptest是否在p0的边链的某个边的外端点处
             * 如果是，返回相应边,否则返回null
             */
            PO POutside;
            Node<LINE> lnt = p0.ll.Head;
            if(p0.shadow_pl==null)
            while (lnt != null)
            {
                if (p0.IsEqualToMe( lnt.Data.p1))
                    POutside = lnt.Data.p2;
                else
                    POutside = lnt.Data.p1;
                if (POutside.IsEqualToMe(ptest))
                    return lnt.Data;
                lnt = lnt.Next;
            }
            else
            {
                info.Insert("here shadow");
                Node<PO> pnt = p0.shadow_pl.Head;
                while(pnt!=null){
                    lnt = pnt.Data.ll.Head;
                    while (lnt != null)
                    {
                        if (p0.IsEqualToMe(lnt.Data.p1))
                            POutside = lnt.Data.p2;
                        else
                            POutside = lnt.Data.p1;
                        if (POutside.IsEqualToMe(ptest))
                            return lnt.Data;
                        lnt = lnt.Next;
                    }
                    pnt = pnt.Next;
                }
            }
            return null;
        }
        private void shadowPO(PO shap,PO shapflag,PO pap,PO papflag)
        {
            /*
             * 将影子点的边转移到父点中去
             * shap是影子点,shapflag是影子点的标记点
             * pap是父点，papflag是父点的标记点
             */
            util u1 = new util();
            if (pap.ll.Head.Data.Po_insideme(papflag))
            {//如果标记点在父点的首边上
                if (shap.ll.Head.Data.Po_insideme(shapflag))
                {
                    #region case1
                    Node<LINE> lnt = shap.ll.Head;
                    while (lnt != null)
                    {
                        //用父点换掉边中的影子点
                        lnt.Data.Po_insideme_and_changeit(pap);
                        pap.ll.Head_Insert(lnt.Data);
                        lnt = lnt.Next;
                    }
                    #endregion case1
                }
                else
                {
                    #region case2
                    LinkList<LINE> lt = new LinkList<LINE>();
                    Node<LINE> lnt = shap.ll.Head;
                    while (lnt != null)//将shap的线链反序
                    {
                        lt.Head_Insert(lnt.Data);
                        lnt = lnt.Next;
                    }
                    lnt = lt.Head;
                    while (lnt != null)
                    {
                        lnt.Data.Po_insideme_and_changeit(pap);
                        pap.ll.Head_Insert(lnt.Data);
                        lnt = lnt.Next;
                    }
                    #endregion case2
                }
            }
            else
            {//如果标记点在父点的尾边上
                if (shap.ll.Head.Data.Po_insideme(shapflag))
                {
                    shap.ToString();
                    #region case3
                    Node<LINE> lnt = shap.ll.Head;
                    while (lnt != null)
                    {
                        lnt.Data.Po_insideme_and_changeit(pap);
                        pap.ll.Insert(lnt.Data);
                        lnt = lnt.Next;
                    }
                    #endregion case3
                }
                else
                {
                    #region case4
                    LinkList<LINE> lt = new LinkList<LINE>();
                    Node<LINE> lnt = shap.ll.Head;
                    while (lnt != null)//将shap的线链反序
                    {
                        lt.Head_Insert(lnt.Data);
                        lnt = lnt.Next;
                    }
                    lnt = lt.Head;
                    while (lnt != null)
                    {
                        lnt.Data.Po_insideme_and_changeit(pap);
                        pap.ll.Insert(lnt.Data);
                        lnt = lnt.Next;
                    }
                    #endregion case4
                }
            }
            work_pl.Delete(shap);
            //如果此时父节点的尾边封闭，则开启其尾边相应方向
            if (!pap.ll.Last.Data.negtive && !pap.ll.Last.Data.positive&&pkey.key!=3)
                Open_direction(pap.ll,pap);
        }
        private void Insert1()
        {
            util u1 = new util();
            LINE l0 = nt_current.Data;
            pg.ToPrint(info);
            /*获取线链的头边和尾边的外端点*/
            PO PLa, PFa;
            if (pkey.IsEqualToMe(l0.p1))
                PLa = l0.p2;
            else
                PLa = l0.p1;
            if (pkey.IsEqualToMe(pre_l.Head.Data.p1))
                PFa = pre_l.Head.Data.p2;
            else
                PFa = pre_l.Head.Data.p1;
            if (pg.selected) info.Insert("po selected");
            else
                info.Insert("po not selected");
            LINE lt;//获取相应边
            info.Insert("t4");
            #region PLa-PFa已存在
            //如果PLa-PFa已存在,且尾边位于首边未搜索侧或首边两侧都搜索过
            lt = Is_OutSide(PLa, PFa);
            if (lt != null) info.Insert("ok");
            if (lt != null&&(u1.noSearched(pre_l.Head.Data,PLa)||!pre_l.Head.Data.positive&&!pre_l.Head.Data.negtive)&&pkey.key!=3)
            {
                info.Insert("t1");
                pkey.non_merged = false;
                Set_direction(pre_l.Head.Data, PLa);
                Set_direction(pre_l.Last.Data, PFa);
                Set_direction(lt, pkey);
                nt_current = nt_current.Next;
                // u1.InFile(u1.tri_num, (numbertri++).ToString() + '\t' + pkey.number + '\t' + PLa.number + '\t' + PFa.number);
                return;
            }
            #endregion
            //后面的没有PLa-PFa已存在的情况
            #region pg位于其已搜索侧
            if (!u1.noSearched(pre_l.Head.Data, pg)&&u1.noSearched(pre_l.Head.Data,PLa) || pg == PFa)
            {
                //pg是当前边首边的外端点
                info.Insert("t3");
                pkey.non_merged = false;
                Set_direction(pre_l.Head.Data, PLa);
                Set_direction(l0, PFa);
                AddLine(PFa, PLa, pkey);
                nt_current = nt_current.Next;
                return;
                //u1.InFile(u1.tri_num, (numbertri++).ToString() + '\t' + pkey.number + '\t' + PLa.number + '\t' + PFa.number);
            }
            #endregion
            if (pg.selected)
            {
                //如果pg已经被选过
                #region delete shadowpo
                //当前被处理的点的影子点位于pg的边链上
                LINE shadow_l = Is_OutSide(pg, pkey);
                if (shadow_l != null)
                {
                    info.Insert("shadow_pg_side");
                    PO shadow_po = shadow_l.getanotherpo(pg);
                    PO shadow_po1, shadow_po2, shadow_pokey;
                    shadow_po1 = shadow_po.ll.Head.Data.getanotherpo(shadow_po);
                    shadow_po2 = shadow_po.ll.Last.Data.getanotherpo(shadow_po);
                    float shadow_ang1, shadow_ang2;
                    shadow_pokey = shadow_po1;
                    shadow_ang1 = u1.d2_Cal_Op_Angle(PLa, shadow_po1, pkey);
                    shadow_ang2 = u1.d2_Cal_Op_Angle(PLa, shadow_po2, pkey);
                    if (shadow_ang2 < shadow_ang1)
                        shadow_pokey = shadow_po2;
                    lt = Is_OutSide(shadow_pokey, PLa);
                    if (lt == null)
                        lt = AddLine(shadow_pokey, PLa, shadow_po);
                    PAINT.GetLL(lt);
                    shadowPO(shadow_po, shadow_pokey, pkey, PLa);
                    nt_current = pkey.ll.Last;
                    return;
                }
                #endregion
                #region last's outside
                //如果pg是last的边链的外端点
                lt = Is_OutSide(PLa, pg);
                if (lt != null)
                {
                    info.Insert("last's  outside\n");
                    Set_direction(pre_l.Last.Data, pg);
                    AddLine(pkey, pg, PLa);
                    //以上两句代码的顺序不能反，否则会出错
                    Set_direction(lt, pkey);
                    nt_current = nt_current.Next;
                    return;
                }
                #endregion last'soutside
                #region shadow_deal
                    #region create shadowpo
                    //如果pg是非特殊位置
                    info.Insert("arbitrary POint\n");
                    PO pt = new PO(pg.x, pg.y, pg.z);
                    if (pg.shadow_pl != null)
                        pg.shadow_pl.Insert(pt);
                    else
                    {
                        info.Insert("add new shadow po\n");
                        pg.shadow_pl = new LinkList<PO>();
                        pg.shadow_pl.Insert(pg);
                        pg.shadow_pl.Insert(pt);
                    }
                    pt.shadow_pl = pg.shadow_pl;                
                    pg = pt;
                    //u1.InFile(u1.infopath, "here1");
                    AddLine(pkey, pg, PLa);
                    // u1.InFile(u1.infopath, "here2");
                    AddLine(PLa, pg, pkey);
                    //u1.InFile(u1.infopath, "here3");
                    Set_direction(l0, pg);
                    // u1.InFile(u1.infopath, "here4");
                    //u1.InFile(u1.tri_num, (numbertri++).ToString() + '\t' + pkey.number + '\t' + PLa.number + '\t' + pg.number);
                    work_pl.Insert(pg);
                    //u1.InFile(u1.infopath,"here5");
                    pg.selected = true;
                    nt_current = nt_current.Next;
                    #endregion
                #endregion
            }
            else
            {
                #region ordinary_po
                info.Insert("ordinary point"+pg.number);
                //pg作为普通点处理
                pg.number = number++;
                pg.selected = true;
                //u1.InFile(u1.node_key, pg.number.ToString() + '\t' + pg.x + '\t' + pg.y + '\t' + pg.key);
                AddLine(pkey, pg, PLa);
                AddLine(PLa, pg, pkey);
                Set_direction(l0, pg);
                //u1.InFile(u1.tri_num, (numbertri++).ToString() + '\t' + pkey.number + '\t' + PLa.number + '\t' + pg.number);
                if (pg.key != 3)
                    work_pl.Insert(pg);
                //work_pl.Dispaly();
                nt_current = nt_current.Next;
                #endregion
            }
        }
        private LINE Seek_line_in_initial_line(PO p1, PO p2)
        {
            Node<LINE> ln = initial_ll.Head;
            if (initial_ll.Head == null) return null;
            util u1 = new util();
            while (ln != null)
            {
                if (ln.Data.Po_insideme(p1) && ln.Data.Po_insideme(p2))
                {
                    initial_ll.Delete(ln.Data);
                    return ln.Data;
                }
                ln = ln.Next;
            }
            return null;
        }
        private void Set_direction(LINE l0, PO p0)
        {
            /*
             * 根据p0与s的相互关系，决定s的某个方向上是否有效
             * 设定为无效，则对应的标志为
             */
            util u1 = new util();
            float f;
            f = u1.Direct_2d(l0, p0);
            if (f > 0)
                l0.positive = false;
            else if (f < 0)
                l0.negtive = false;
        }
        private void Open_direction(LinkList<LINE> l_l,PO pk)
        {
            //开启尾边的未搜索方向
            Node<LINE> ltail, ltailp,lt;
            lt = l_l.Head;
            ltail = null;
            ltailp = null;
            while(lt.Next!=null)
            {
                ltailp = lt;
                ltail = lt.Next;
                lt = lt.Next;
            }//找到最后的两个节点
            PO pt;
            pt = ltailp.Data.getanotherpo(pk);
            util u1 = new util();
            if (u1.Direct_2d(ltail.Data, pt) > 0)
                ltail.Data.negtive = true;
            else
                ltail.Data.positive = true;
        }
        private LINE AddLine(PO p1, PO p2, PO px)
        {

            //在p1和p2之间添加一条边，px用于确定p1-p2需要失效的方向,并返回新生成的边
            //create the info for k1 and k2
            util u1 = new util();
            LINE ln0;
            ln0 = Seek_line_in_initial_line(p1, p2);
            if (ln0 == null)
            {
                ln0 = new LINE(p1, p2);
            }
            else
            {
                ln0.p1 = p1;
                ln0.p2 = p2;//防止出现了影子点的特殊情况
            }
            PAINT.GetLL_triangle(ln0);
            Set_direction(ln0, px);
            //put ln0 into k1
            if (p1.ll.Head == null)
            {
                //u1.InFile(u1.infopath, "insert in p1");
                p1.ll.Insert(ln0);
            }
            else
                if (p1.ll.Last.Data.p1.IsEqualToMe(px) || p1.ll.Last.Data.p2.IsEqualToMe(px))
                    p1.ll.Insert(ln0);
                else
                    p1.ll.Head_Insert(ln0);
            //put ln0 into k2
            if (p2.ll.Head == null)
            {
                //u1.InFile(u1.infopath, "insert in p2");
                p2.ll.Insert(ln0);
            }
            else
                if (p2.ll.Last.Data.p1.IsEqualToMe(px) || p2.ll.Last.Data.p2.IsEqualToMe(px))
                    p2.ll.Insert(ln0);
                else
                    p2.ll.Head_Insert(ln0);
            return ln0;
        }
        void SearchD(){
            util u1=new util();
		    soliangle1 = 0;
            int i, j;
            float d;
		    d = 3;
		    pg = null;
            LINE l0 = nt_current.Data;
            Node<PO> pn;
            Node<PO> cn;
		    do{
			    Two_po_netnumberlist(l0, d);
                pn = net_choosed.Head;
			    while (pn!=null)
			    {
				    if (pn.Data.x>= 0 && pn.Data.x< Mx&&pn.Data.y>= 0 && pn.Data.y< Ny){
					    i = (int)pn.Data.x;
					    j = (int)pn.Data.y;
					    cn = DataNet[i,j].Head;
					    while (cn!=null){
                            soliangle2 = u1.d2_Cal_Op_Angle(l0,cn.Data);
                            if (soliangle1 < soliangle2&&u1.noSearched (l0,cn.Data))
                            {
                                //u1.InFile(u1.infopath,"change it");
                                soliangle1 = soliangle2;
                                pg = cn.Data;
                            }
						    cn = cn.Next;
					    }
				    }
				    pn=pn.Next;
			    }
			    d += Xi;
		    } while (d<5 &&pg == null);
	    }
        public void Initial()
        {
            //m从生成某个模型的点集中获取包含一个初始面的点集
            util u1 = new util();
            PO p0, p1;
            LINE l1;
            Node<PO> pn =initial_pl.Head;
            initial_side_num = 0;
            p0 = pn.Data;
            p0.key = 3; 
            p0.selected = true;
            work_pl.Insert(p0);
            initial_side_num++;
            while (pn.Next != null)
            {
                //获取初始面的三个顶点和一个初始面
                p1 = pn.Next.Data;
                p1.key = 3;
                //加入初始的点，构成初始点集
                //p1.selected = true;
                work_pl.Insert(p1);
                initial_side_num++;
                //产生初始三角形的一条边
                l1 = new LINE(p0, p1);
                Set_direction(l1, initial_center);
                //为每个点加入初始的边集
                initial_ll.Insert(l1);
                PAINT.GetLL(l1);
                pn = pn.Next;
                p0 = p1;
            }
            p1 = initial_pl.Head.Data;
            //产生初始三角形的一条边
            l1 = new LINE(p0, p1);
            Set_direction(l1, initial_center);
            //为每个点加入初始的边集
            initial_ll.Insert(l1);
            PAINT.GetLL(l1);
            work_pl.Head.Data.ll.Insert(initial_ll.Head.Data);
        }
        public void SearchLine()
        {
            //Delaunay算法的主函数，遍历边链的每一条边，并分别进行处理
            util u1 = new util();
            nt_current = pre_l.Last;
            info.Insert("--start SearchLine--\n");
            int num = 20;
            //如果一个边链一开始就已经封闭，则开启尾边的相应方向
            if (!nt_current.Data.negtive && !nt_current.Data.positive)
                Open_direction(pre_l,pkey);           
            for (int i = 0; i < num; i++)
            {
                if (nt_current == null)
                    break;
                if (nt_current.Data.non_merged)
                {
                    nt_current.Data.ToPrint(info);
                    SearchD();
                    if (pg != null)
                    {
                        Insert1();
                    }
                    else
                    {
                        info.Insert("no POint finded\n");
                        break;
                    }
                }
                else
                {
                    info.Insert("-----has been merged-----\n");
                }
            }
            info.Insert("--SearchLine end--\n");
        }
        public void SearchPoint()
        {
            //Delaunay算法的主函数，遍历边链的每一条边，并分别进行处理
            util u1 = new util();
            u1.Tip_Time();
            Node<PO> pt =work_pl.Head;
            info.Insert("--start SearchPOint--");
            loc = 0;
            while(pt!=null)
            {
                loc++;
                if (pt == null)
                    break;
                if (STOP)
                    break;
                pkey = pt.Data;
                pt.Data.ToPrint(info);//输出当前需要处理的点
                pre_l = pt.Data.ll;
                SearchLine();
                PEBIGrid(pt.Data);
                pt = pt.Next;
            }
            
            info.Insert("--SearchPOint end--\n");
            u1.Span_from_last_time("runtime");
        }
        public void SearchPoint_step(int stepx)
        {
            //Delaunay算法的主函数，遍历边链的每一条边，并分别进行处理
            util u1 = new util();
            Node<PO> pt = work_pl.Head;
            info.Insert("--start SearchPOint--\n");
            for (int i =0; i < stepx; i++)
            {
                info.Insert(":" + "-------" + i + "----------\n");
                if (pt == null)
                    break;
                if (STOP) break;
                loc = i;
                pkey = pt.Data;
                pt.Data.ToPrint(info);//输出当前需要处理的点
                pre_l = pt.Data.ll;
                SearchLine(); 
                PEBIGrid(pt.Data);
                pt = pt.Next;
                Where_pointer = pt;
            }
            info.Insert("--SearchPOint end--");
        }
        public void SearchPoint_next_step()
        {
            //Delaunay算法的主函数，遍历边链的每一条边，并分别进行处理
            util u1 = new util();
            Node<PO> pt = Where_pointer;
            info.Insert("--start SearchPOint_next_step--");
            if (pt == null)
                return;
            loc++;
            info.Insert(":" + "-------" + loc + "----------");
            pkey = pt.Data;
            pt.Data.ToPrint(info);//输出当前需要处理的点
            pre_l = pt.Data.ll;
            SearchLine();
            PEBIGrid(pt.Data);
            pt = pt.Next;
            Where_pointer = pt;
            PAINT.p_tags = pkey;
            PAINT.pl = pl;
            info.Insert("--SearchPOint_next_step end--");
        }
        public void Start()
        {
            /*
             * 1.GetOGL(OGL pa);
             * 2.GetPL(LinkList<PO> pl0);
             * 3.Get_initialpl(LinkList<PO> pl0);
             */
            Initial();
            Into_DataNet();
            SearchPoint();
        }
        public void Start_step(int stepx)
        {
            /*
             * 1.GetOGL(OGL pa);
             * 2.GetPL(LinkList<PO> pl0);
             * 3.Get_initialpl(LinkList<PO> pl0);
             */
            Initial();
            Into_DataNet();
            SearchPoint_step(stepx);
        }
    }
}
