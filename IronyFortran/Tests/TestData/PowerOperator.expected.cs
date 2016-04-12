public string PART06_DRUCKDIFFERENZ(ref double MPW_H, ref double MPW_K, ref double DPW_H, ref double DPW_K)
{
   string PART06_DRUCKDIFFERENZ = String.Empty;
   double KV = 0;
   double X = 0;
   double ROHW = 0;
   ROHW=(double)(1000);
   KV=(double)(1.1547);
   X=(double)(MPW_K);
   X=(double)(DPW_K);
   DPW_H=(double)(Math.Pow((double)(MPW_H/ROHW/KV),(double)2)*ROHW/1000*100);
   PART06_DRUCKDIFFERENZ=@"";
   if(DPW_H==0)PART06_DRUCKDIFFERENZ=@"Druckverlust Ronda muss > 0 sein";
   return PART06_DRUCKDIFFERENZ;
}
