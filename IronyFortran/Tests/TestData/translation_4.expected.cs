public override int TGA_810(ref string XTGA, ref VDIArray<string> ARRAY_810)
{
   int TGA_810 = 0;
   string TGA = String.Empty;
   string S = String.Empty;
   string BEST = String.Empty;
   string D3F = String.Empty;
   string D5F = String.Empty;
   string D3N = String.Empty;
   string D5N = String.Empty;
   string S16 = String.Empty;
   string S30 = String.Empty;
   string S40 = String.Empty;
   string S45 = String.Empty;
   string S70 = String.Empty;
   string S71 = String.Empty;
   string TYP = String.Empty;
   VDIArray<string> STELLE12 = new VDIArray<string>();
   VDIArray<string> STELLE13 = new VDIArray<string>();
   int I16 = 0;
   int I30 = 0;
   int I40 = 0;
   int I45 = 0;
   int I70 = 0;
   int I71 = 0;
   int BL = 0;
   int BH = 0;
   VDIArray<int> ARI40 = new VDIArray<int>();
   VDIArray<int> ARI45 = new VDIArray<int>();
   int I = 0;
   int J = 0;
   int K = 0;
   int L = 0;
   int A = 0;
   int E = 0;
   VDIArray<int> STELLE10 = new VDIArray<int>();
   bool L16 = false;
   bool L30 = false;
   bool L40 = false;
   bool L45 = false;
   bool L70 = false;
   bool L71 = false;
   bool B1 = false;
   bool OK = false;
   D3F=(string)(@"???");
   D5F=(string)(@"?????");
   D3N=(string)(@"000");
   D5N=(string)(@"00000");
   ARI40.SetRange(1, 5, 10,11,16,18,28);
   ARI45.SetRange(1, 10, 1,2,3,4,5,6,5,6,11,12);
   STELLE10.SetRange(1, 8, 1,2,3,2,5,5,5,5);
   STELLE12.SetRange(1, 8, @"1",@"1",@"1",@"Q",@"Q",@"Q",@"S",@"Q");
   STELLE13.SetRange(1, 8, @"0",@"0",@"0",@"6",@"1",@"6",@"1",@"3");
   TGA=(string)(VDISTRING(@"&|&",XTGA,@"??????????????????????????????????????????????????????????"));
   S16=(string)(VDICSTRING(TGA,0,7,9));
   S30=(string)(VDICSTRING(TGA,0,19,21));
   S40=(string)(VDICSTRING(TGA,0,28,30));
   S45=(string)(VDICSTRING(TGA,0,31,33));
   S70=(string)(VDICSTRING(TGA,0,46,50));
   S71=(string)(VDICSTRING(TGA,0,54,58));
   L16=(bool)(S16==D3F || S16==D3N);
   L30=(bool)(S30==D3F || S30==D3N);
   L40=(bool)(S40==D3F || S40==D3N);
   L45=(bool)(S45==D3F || S45==D3N);
   L70=(bool)(S70==D5F || S70==D5N);
   L71=(bool)(S71==D5F || S71==D5N);
   if(L16)S16=(string)(@"1");
   I16=(int)(VDIISTRING(S16,0,0,0));
   B1=(bool)(I16<1 || I16>14);
   if(B1)I16=1;
   if(L30)S30=(string)(@"1");
   I30=(int)(VDIISTRING(S30,0,0,0));
   B1=(bool)(I30<1 || I30>4);
   if(B1)I30=1;
   if(L40)S40=(string)(@"10");
   I40=(int)(VDIISTRING(S40,0,0,0));
   B1=(bool)(true);
   I=1;
   while(B1)
   {
      OK=(bool)(I40==ARI40[I]);
      if(OK)
      {
         B1=(bool)(false);
      }
      else
      {
         I=(int)(I+1);
         B1=(bool)(I<6);
      }
   }
   OK=(bool)(!OK);
   if(OK)I=1;
   if(L45)
   {
      I45=(int)(ARI45[(I-1)*2+1]);
   }
   else
   {
      I45=(int)(VDIISTRING(S45,0,0,0));
   }
   B1=(bool)(true);
   J=(int)((I-1)*2+1);
   K=J;
   while(B1)
   {
      OK=(bool)(I45==ARI45[J]);
      if(OK)
      {
         B1=(bool)(false);
      }
      else
      {
         J=(int)(J+1);
         B1=(bool)(J<K+2);
      }
   }
   OK=(bool)(!OK);
   if(OK)J=K;
   I40=(int)(ARI40[I]);
   I45=(int)(ARI45[J]);
   if(L70)S70=(string)(@"1");
   I70=(int)(VDIISTRING(S70,0,0,0));
   B1=(bool)(I70<1 || I70>126);
   if(B1)I70=1;
   A=(int)(VDIERSTER(@"700",I70,@"710.01"));
   E=(int)(VDILETZTER(@"700",I70,@"710.01"));
   if(L71)
   {
      I71=A;
   }
   else
   {
      I71=(int)(VDIISTRING(S71,0,0,0));
   }
   B1=(bool)(I71<A || I71>E);
   if(B1)I71=A;
   BL=(int)(VDIIWERT(@"700",I70,@"710.01",I71,3));
   BH=(int)(VDIIWERT(@"700",I70,4));
   TYP=(string)(VDICWERT(out L, @"700",I70,6));
   S=(string)(VDISTRING(@"D1$$$$~##",TYP,BL/50));
   BEST=(string)(VDISTRING(@"D1$~###~#|#0$|$0A0",VDICSTRING(TYP,0,1,1),BH,BL/50,STELLE10[I],STELLE12[I],STELLE13[I]));
   TGA_810=2;
   ARRAY_810[1]=VDISTRING(@"810;1;&;&;",BEST,S);
   ARRAY_810[2]=@"810;2;";
   XTGA=(string)(VDISTRING(@"003001~##001000000~##000000~##~##000001000000~####000~####",I16,I30,I40,I45,I70,I71));
   return TGA_810;
}
