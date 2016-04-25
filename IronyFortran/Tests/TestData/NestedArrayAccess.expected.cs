public string PART06_DRUCKDIFFERENZ(ref double MPW_H, ref double MPW_K, ref double DPW_H, ref double DPW_K)
{
   string PART06_DRUCKDIFFERENZ = String.Empty;
   VDIArray<double> ARL = new VDIArray<double>();
   VDIArray<double> ARKV = new VDIArray<double>();
   double L = 0d;
   double KV = 0d;
   int I700 = 0;
   int I710 = 0;
   int TYP = 0;
   int I1 = 0;
   int I2 = 0;
   ARL.SetRange(1, 12, 700, 1500, 3500, 4900, 700, 1500, 2800, 2800, 700, 1500, 3500, 4900);
   ARL.SetRange(13, 24, 700, 1500, 2800, 2800, 700, 1500, 3500, 4900, 700, 1500, 2800, 2800);
   ARKV.SetRange(1, 12, 4.8507, 4.3644, 3.6037, 3.1944, 3.8633, 3.288, 2.8144, 2.8144, 4.4721, 4.264, 3.8348, 3.6037);
   ARKV.SetRange(13, 24, 2.7512, 2.6247, 2.4666, 2.4666, 5.0637, 4.9088, 4.4721, 4.264, 4.3437, 4.0825, 3.7796, 3.7796);
   KV = (double)(MPW_K);
   I700 = (int)(VDIINDEX(@"700"));
   I710 = (int)(VDIINDEX(@"710.01"));
   TYP = (int)(VDIIWERT(@"700", I700, 17));
   TYP = (int)((TYP - 1) * 4 + 1);
   L = (double)(VDIRWERT(@"700", I700, @"710.01", I710, 3) - VDIRWERT(@"700", I700, 11));
   if(L < ARL[TYP])
   {
      KV = (double)(ARKV[TYP]);
   }
   else if(L > ARL[TYP + 3])
   {
      KV = (double)(ARKV[TYP + 3]);
   }
   else
   {
      I1 = (int)(TYP);
      I2 = (int)(TYP + 1);
      while(L > ARL[I2])
      {
         I1 = (int)(I1 + 1);
         I2 = (int)(I2 + 1);
      };
      KV = (double)((L - ARL[I1]) / (ARL[I2] - ARL[I1]) * (ARKV[I2] - ARKV[I1]) + ARKV[I1]);
   };
   DPW_H = (double)( Math.Pow((MPW_H / KV / 1000), 2) * 100);
   DPW_K = (double)(0);
   PART06_DRUCKDIFFERENZ = (string)(@"");
   return PART06_DRUCKDIFFERENZ;
}

