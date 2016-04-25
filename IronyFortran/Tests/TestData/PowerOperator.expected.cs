public string PART06_DRUCKDIFFERENZ(ref double MPW_H, ref double MPW_K, ref double DPW_H, ref double DPW_K)
{
   string PART06_DRUCKDIFFERENZ = String.Empty;
   double KV = 0d;
   double X = 0d;
   double ROHW = 0d;
   ROHW = (double)(1000);
   KV = (double)(1.1547);
   X = (double)(MPW_K);
   X = (double)(DPW_K);
   DPW_H = (double)( Math.Pow((MPW_H / ROHW / KV), 2) * ROHW / 1000 * 100);
   PART06_DRUCKDIFFERENZ = (string)(@"");
   if(DPW_H == 0) PART06_DRUCKDIFFERENZ = (string)(@"Druckverlust Ronda muss > 0 sein");
   return PART06_DRUCKDIFFERENZ;
}

