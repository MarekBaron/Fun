public override int TGA_810(ref string XTGA, ref VDIArray<string> ARRAY_810)
{
   int TGA_810 = 0;
   string TGA = String.Empty;
   string S = String.Empty;
   VDIArray<string> MASK = new VDIArray<string>();
   string TEMP = String.Empty;
   string S16 = String.Empty;
   string S40 = String.Empty;
   string S45 = String.Empty;
   string S70 = String.Empty;
   string S71 = String.Empty;
   string S76 = String.Empty;
   string D5F = String.Empty;
   string D5N = String.Empty;
   string D3F = String.Empty;
   string D3N = String.Empty;
   string GNR = String.Empty;
   string TYP = String.Empty;
   int I40 = 0;
   int I45 = 0;
   int I70 = 0;
   int I71 = 0;
   int L = 0;
   int BH = 0;
   int BL = 0;
   bool L70 = false;
   bool L71 = false;
   bool B1 = false;
   bool B2 = false;
   bool L40 = false;
   bool L45 = false;
   TEMP = (string)(@"ala ma ""kota""");
   GNR = (string)(@"PH0");
   D3F = (string)(@"???");
   D3N = (string)(@"000");
   D5F = (string)(@"?????");
   D5N = (string)(@"00000");
   MASK.SetRange(1, 5, @"x-x----------", @"--------xx---", @"--------xx---", @"--------xx---", @"-----------xx");
   TGA = (string)(VDISTRING(@"&|&", XTGA, @"??????????????????????????????????????????????????????????"));
   S16 = (string)(VDICSTRING(TGA, 0, 7, 9));
   S40 = (string)(VDICSTRING(TGA, 0, 28, 30));
   S45 = (string)(VDICSTRING(TGA, 0, 31, 33));
   S70 = (string)(VDICSTRING(TGA, 0, 46, 50));
   S76 = (string)(VDICSTRING(TGA, 0, 51, 53));
   S71 = (string)(VDICSTRING(TGA, 0, 54, 58));
   L40 = (bool)(S40 == D3F || S40 == D3N);
   L45 = (bool)(S45 == D3F || S45 == D3N);
   L70 = (bool)(S70 == D5F || S70 == D5N);
   L71 = (bool)(S71 == D5F || S71 == D5N);
   if(L40) S40 = (string)(@"0");
   if(L45) S45 = (string)(@"0");
   if(L70) S70 = (string)(@"1");
   if(L71) S71 = (string)(@"0");
   I40 = (int)(VDIISTRING(S40, 0, 0, 0));
   I45 = (int)(VDIISTRING(S45, 0, 0, 0));
   I70 = (int)(VDIISTRING(S70, 0, 0, 0));
   I71 = (int)(VDIISTRING(S71, 0, 0, 0));
   if(ARRAY_810[1])
   {
      I40 = (int)( Math.Pow(1, 0));
      I45 = (int)(0);
   }
   else
   {
      I40 = (int)(VDIISTRING(S40, 0, 0, 0));
      B1 = (bool)(I40 < 6 && I45 > 0 && I45 < 14);
      if(B1)
      {
         S = (string)(VDICSTRING(MASK[I40], MASK[1], I45, I45));
         B2 = (bool)(S == @"-");
         if(B2)
         {
            I40 = (int)(1);
            I45 = (int)(0);
         }
         else if(B1)
         {
            I40 = (int)(2);
         };
      }
      else
      {
         I40 = (int)(1);
         I45 = (int)(0);
      };
   };
   B1 = (bool)(I70 > 10);
   if(B1) I70 = (int)(10);
   if(B1) I70 = (int)int.Parse(@"10");
   L71 = (bool)(I71 == 0);
   if(L71) I71 = (int)(VDIERSTER(@"700", I70, @"710.01"));
   TYP = (string)(VDICSTRING(VDICWERT(out L, @"700", I70, 6), 0, 5, 6));
   BH = (int)(VDIIWERT(@"700", I70, 4));
   BL = (int)(VDIIWERT(@"700", I70, @"710.01", I71, 3));
   S = (string)(VDISTRING(@"&$$~#~#", GNR, TYP, BH / 100, BL / 100));
   XTGA = (string)(VDISTRING(@"001001&00100000000100000~##~##000001000000~####$$$~####", S16, I40, I45, I70, S76, I71));
   ARRAY_810[1] = VDISTRING(@"810;1;&;&;;;;", S, S);
   ARRAY_810[2] = @"810;2;";
   TGA_810 = (int)(2);
   return TGA_810;
}

