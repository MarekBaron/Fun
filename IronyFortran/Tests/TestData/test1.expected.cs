int TGA_810(ref string XTGA, ref VDIArray<string> ARRAY_810)
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
   return TGA_810;
}

