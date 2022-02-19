using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Numpy;
using System.IO;
using NAudio.Wave;


namespace DataExperimentalNew
{
    public partial class DataExperimentalNew : Form
    {

        List<double> wav = new List<double>();
        public DataExperimentalNew()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Задание 1
        /// </summary>
        private List<double> Linear(int N, double a, double b)
        {
            List<double> x = new List<double>();
            for (double t = 1; t < N; t++)
            {
                double temp = a * t + b;
                x.Add(temp);
            }
            return x;
        }
        private List<double> Exponential(int N, double a, double b)
        {
            List<double> x = new List<double>();
            for (double t = 1; t < N; t++)
            {
                double temp = b * Math.Exp(-a * t);
                x.Add(temp);
            }
            return x;
        }
        /// <summary>
        /// Задание 2
        /// </summary>
        private List<double> EmbedRandom()
        {
            var min = -1;
            var max = 1;
            int N = 10000, S = 1;
            List<double> lis = new List<double>();
            Random rnd = new Random();
            for (double x = 0; x < N; x += 10)
            {
                double y = rnd.NextDouble() * (S * max - S * min) + S * min;
                lis.Add(y);
            }
            return lis;

        }
        private List<double> EmbedRandom2()
        {
            var min = -5;
            var max = 5;
            int N = 10000, S = 5;
            List<double> lis = new List<double>();
            Random rnd = new Random();
            for (double x = 0; x < N; x += 10)
            {
                double y = rnd.NextDouble() * (S * max - S * min) + S * min;
                lis.Add(y);
            }
            return lis;

        }
        private List<double> SelfRandom()
        {
            var min = -1;
            var max = 1;
            double mt = 0;
            int N = 10000, S = 1;
            double milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            double remainder = milliseconds % 10000;
            List<double> lis = new List<double>();
            for (double x = 0; x <= N; x += 10)
            {
                mt = mt + Math.Pow(remainder, 2) * 3 * 787;
                remainder = mt % 1000000000;
                double y = (remainder / 999999999) * (S * max - S * min) + S * min;
                lis.Add(y);
            }
            return lis;

        }
        /// <summary>
        /// Задание 3
        /// </summary>
        private void Harmonic()
        {
            List<double> x = EmbedRandom();
            var min = x.Min();
            textBox1.Text = min.ToString();
            var max = x.Max();
            textBox2.Text = max.ToString();
            var N = x.Count();
            ////////////////////////////////////////////
            double m = 0;
            for (int k = 0; k < N; k++)
            {
                m = x[k] + m;
            }
            double average = 1.0 / N * m;
            textBox3.Text = average.ToString();
            //////////////////////////////////////////// мат ожидание
            double Dn = 0;
            for (int k = 0; k < N; k++)
            {
                Dn = Dn + Math.Pow((x[k] - average), 2);
            }
            double D = 1.0 / N * Dn;
            textBox4.Text = D.ToString();
            //////////////////////////////////////////// дисперсия
            double gamma = Math.Sqrt(D);
            textBox5.Text = gamma.ToString();
            //////////////////////////////////////////// Стандартное отклонение
            double psin = 0;
            for (int k = 0; k < N; k++)
            {
                psin = psin + Math.Pow((x[k]), 2);
            }
            double psi = 1.0 / N * psin;
            textBox6.Text = psi.ToString();
            //////////////////////////////////////////// Средний квадрат
            double E = Math.Sqrt(psi);
            //////////////////////////////////////////// Средне квадрт ошибк
            //////////////////////////////////////////// 
            double asymmn = 0;
            for (int k = 0; k < N; k++)
            {
                asymmn = asymmn + Math.Pow((x[k] - average), 3);
            }
            double asymm = 1.0 / N * asymmn;
            textBox7.Text = asymm.ToString();
            //////////////////////////////////////////// Ассиметрия
            double asymm_k = asymm / Math.Pow(gamma, 3);
            textBox8.Text = asymm_k.ToString();
            //////////////////////////////////////////// коэфициент ассиметрии
            ///  //////////////////////////////////////////// 
            double excessn = 0;
            for (int k = 0; k < N; k++)
            {
                excessn = excessn + Math.Pow((x[k] - average), 4);
            }
            double excess = 1.0 / N * excessn;
            textBox9.Text = excess.ToString();
            //////////////////////////////////////////// эксцесс
            ////////////////////////////////////////////
            double curtosis = (excess / Math.Pow(gamma, 4)) - 3;
            textBox10.Text = curtosis.ToString();
            //////////////////////////////////////////// куртозис
            //LinearUpward(); /// график
        }

        private bool stationarity()
        {
            int M = 1000;
            int e = 10;
            List<double> x = EmbedRandom();
            var N = x.Count();
            // var prev_average = 0.0;
            //var prev_D = 0.0;
            List<double> Mlist = new List<double>();
            List<double> Dlist = new List<double>();
            double m = 0;
            double Dn = 0;
            for (int i = 0; i < M; i++)
            {
                //0-10 10-20 20-30
                for (int k = i * (N / M); k < i * N / M + N / M; k++)
                {
                    m = x[k] + m;
                }
                double average = m / N;
                Mlist.Add(average);
                //////////////////////////////////////////// мат ожидание
                for (int k = i * M; k < i * N / M + N / M; k++)
                {
                    Dn = Dn + Math.Pow((x[k] - average), 2);
                }
                double D = Dn / N;
                Dlist.Add(D);
                for (int j = 0; j < i - 1; j++)
                {
                    if (i != 0)
                    {
                        if ((Math.Max(Mlist[i], Mlist[j]) - Math.Min(Mlist[i], Mlist[j])) / Math.Max(Mlist[i], Mlist[j]) * 100 > e)
                        {
                            return false;
                        }
                        if ((Math.Max(Dlist[i], Dlist[j]) - Math.Min(Dlist[i], Dlist[j])) / Math.Max(Dlist[i], Dlist[j]) * 100 > e)
                        {
                            return false;
                        }

                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Задание 4
        /// </summary>
        private List<double> avtocovariance(List<double> x)
        {
            double avgN = x.Average();
            double Num = x.Count();
            List<double> Y = new List<double>();

            for (int i = 0; i < Num; i++)
            {
                double sum1 = 0;
                double sum2 = 0;
                for (int j = 0; j < Num - i; j++)
                {
                    sum1 += (x[j] - avgN) * (x[j + i] - avgN);
                }
                for (int k = 0; k < Num; k++)
                {
                    sum2 += (x[k] - avgN) * (x[k] - avgN);
                }
                Y.Add(sum1 / sum2);
            }
            return Y;
        }
        private List<double> covariance(List<double> x, List<double> y)
        {
            double avgX = x.Average();
            double avgY = y.Average();
            double Num = x.Count();
            List<double> L = new List<double>();
            for (int i = 0; i < Num; i++)
            {
                double sum1 = 0;
                for (int j = 0; j < Num - i; j++)
                {
                    sum1 += (x[j] - avgX) * (y[j + i] - avgY);
                }
                L.Add(sum1 / Num);
            }
            return L;
        }
        private void density(List<double> prc, ref System.Windows.Forms.DataVisualization.Charting.Chart chart)
        {

            int M = 20;
            var gist = np.histogram(np.array(prc.ToArray()), M);
            chart.Series[0].Points.Clear();
            for (int i = 0; i < gist.Item1.size; i++)
                chart.Series[0].Points.AddXY(gist.Item2[i].asscalar<double>(), gist.Item1[i].asscalar<double>());
        }

        /// <summary>
        /// Задание 5
        /// </summary>
        private List<double> Harm()
        {
            double N = 500;
            double A = 10;
            double f = 3;
            double dt = 0.002;
            double y = 0;
            List<double> y1 = new List<double>();
            for (double i = 0; i < N * dt; i += dt)
            {
                y = A * Math.Sin(2 * Math.PI * f * i);
                y1.Add(y);

            }
            return y1;
        }
        private List<double> Harm2()
        {
            double N = 1000;
            double A = 5;
            double f = 6;
            double dt = 0.001;
            double y = 0;
            List<double> y1 = new List<double>();
            for (double i = 0; i <= N * dt; i += dt)
            {
                y = A * Math.Sin(2 * Math.PI * f * i);
                y1.Add(y);

            }
            return y1;
        }
        private List<double> polyHarm3()
        {
            double N = 1000;
            double[] A = new double[3] { 10, 100, 15 };
            double[] f = new double[3] { 15, 37, 134 };
            double dt = 0.001;
            double y = 0;
            List<double> y2 = new List<double>();
            for (double i = 0; i <= N * dt; i += dt)
            {
                y = 0;
                for (int j = 0; j < A.Length; j++)
                {
                    y = y + A[j] * Math.Sin(2 * Math.PI * f[j] * i);
                }
                y2.Add(y);
            }
            return y2;
        }
        /// <summary>
        /// Задание 6
        /// </summary>
        private List<double> Shift(List<double> x)
        {
            List<double> x2 = new List<double>();
            int cs = 1;
            for (int i = 0; i < x.Count; i++)
            {
                double g = x[i] + cs;
                x2.Add(g);
            }
            return x2;
        }

        private List<double> spikes(List<double> lst)
        {
            Random rnd = new Random();
            int cnt = rnd.Next(1, 5);
            for (int x = 0; x < cnt; x++)
            {
                double tmp = rnd.NextDouble() * (100.0 - 0.2) + 0.2;
                lst[rnd.Next(0, lst.Count())] = tmp * (rnd.Next(1, 3) == 1 ? 1 : -1);
            }
            return lst;
        }
        /// <summary>
        /// Задание 7
        /// </summary>
        private List<double> antishift(List<double> x)
        {
            var tmp = Shift(x);
            List<double> x2 = new List<double>();
            double average = tmp.Average();
            for (int i = 0; i < x.Count; i++)
            {
                double t = tmp[i] - average;
                x2.Add(t);
            }
            return x2;
        }
        private List<double> antispike(List<double> x)
        {
            var tmp = spikes(x);
            for (int i = 1; i < tmp.Count - 1; i++)
            {
                if (Math.Abs(tmp[i] / tmp[i - 1]) > 50)
                    tmp[i] = (tmp[i - 1] + tmp[i + 1]) / 2.0;
                else
                    tmp[i] = tmp[i];
            }
            return tmp;
        }
        /// <summary>
        /// Задание 8
        /// </summary>
        private List<double> additiveTrRand(List<double> x)
        {
            Random rnd = new Random();
            List<double> x2 = new List<double>();
            for (int i = 0; i < x.Count; i++)
            {
                int value = rnd.Next(30, 324);
                x[i] += value;
                x2.Add(x[i]);
            }
            return x2;
        }
        private List<double> HighlightingTrend(List<double> tmp)
        {
            double L = 10;
            int u = tmp.Count;
            List<double> y = new List<double>();
            for (int i = 0; i < tmp.Count - L; i++)
            {
                double sum = 0;
                for (int k = i; k < i + L; k++)
                {
                    sum += tmp[k];
                }
                y.Add(sum / L);
            }
            return y;
        }

        private List<double> HarmZad3(List<double> x)
        {
            List<double> y = Harm2();
            int inr = 100000;
            for (int k = 0; k < inr; k++)
            {
                double[] p = new double[1000];
                List<double> ran = EmbedRandom2();
                for (int i = 0; i < 1000; i++)
                {
                    p[i] = y[i] + ran[i];
                }
                for (int i = 0; i < 1000; i++)
                {
                    x[i] += p[i];
                }
            }
            for (int i = 0; i < 1000; i++)
            {
                x[i] /= inr;
            }
            return x;
        }
        private void AnalyticalError(List<double> x)
        {
            int N = 1000;
            int k = this.trackBar1.Value;
            List<double> rand = EmbedRandom();
            double[] sin = new double[N];
            for (int i = 0; i < N; i++)
                sin[i] = x[i] / k;
            double randSum = 0;
            double randSinSum = 0;
            for (int i = 0; i < N; i++)
            {
                randSum += rand[i];
                randSinSum += sin[i];
            }
            double randAvg = x.Sum() / N;
            double randY = 0;
            double randSinAvg = x.Sum() / N;
            double randSinY = 0;
            for (int i = 0; i < N; i++)
            {
                randY += (rand[i] - randAvg) * (rand[i] - randAvg);
                randSinY += (sin[i] - randSinAvg) * (sin[i] - randSinAvg);
            }
            randY = Math.Sqrt(randY / N);
            randSinY = Math.Sqrt(randSinY / N);

            label1.Text += k + " Стандартное отклонение рандома: " + randY + "\n";
            label1.Text += k + " Стандартное отклонение рандома + син: " + randSinY + "\n";
        }

        //private List<double> Zad2() Смотри в фкнкции button8_Click
        //  private List<double> AntiTrend() Смотри в фкнкции button8_Click

        private List<double> FourierAmplitude(List<double> x)
        {
            int N = x.Count;
            List<double> x2 = new List<double>();
            for (int i = 0; i < N / 2; i++)
            {
                double re = 0;
                double im = 0;
                for (int j = 0; j < N; j++)
                {
                    re += x[j] * Math.Cos((2 * Math.PI * i * j) / N);
                    im += x[j] * Math.Sin((2 * Math.PI * i * j) / N);
                }
                re /= N;
                im /= N;
                double y = Math.Sqrt(Math.Pow(re, 2) + Math.Pow(im, 2));
                x2.Add(y);
            }
            //for (int i = 0; i < x2.Count; i++)
            //{
            //    Console.WriteLine(x2[i]);
            //    Console.ReadLine();
            //}
            return x2;
        }
        private List<double> FourierSpectrum(List<double> x, double window)
        {
            int N = x.Count;
            double wd = ((N - N * window) / 2);
            List<double> x2 = x.ToList();
            List<double> x3 = new List<double>();
            for (int i = 0; i < wd; i++)
            {
                x2[i] = 0;
                x2[N - i - 1] = 0;
            }
            for (int i = 0; i < N / 2; i++)
            {
                double re = 0;
                double im = 0;
                for (int j = 0; j < N; j++)
                {
                    re += x2[j] * Math.Cos((2 * Math.PI * i * j) / N);
                    im += x2[j] * Math.Sin((2 * Math.PI * i * j) / N);
                }
                re /= N;
                im /= N;
                double y = Math.Sqrt(Math.Pow(re, 2) + Math.Pow(im, 2));
                x3.Add(y);
            }
            return x3;

        }
        private List<double> LPFPorter(double fc, int m)
        {
            double[] d = new double[] { 0.35577019, 0.2436983, 0.07211497, 0.00630165 };
            double fact = 2.0 * fc;
            List<double> lpw = new List<double>() { fact };
            double arg = fact * Math.PI;
            lpw.AddRange(Enumerable.Range(1, m).Select(i => Math.Sin(arg * i) / (Math.PI * i)));
            lpw[m] /= 2;
            double sumg = lpw[0];
            for (int i = 1; i <= m; i++)
            {
                double sum = d[0];
                arg = Math.PI * i / m;
                int j = 1;
                lpw[i] *= d.Skip(1).Aggregate(d[0], (acc, val) => acc + 2.0 * val * Math.Cos(arg * j++));
                sumg += 2 * lpw[i];
            }
            lpw = lpw.Select(v => v / sumg).ToList();
            return lpw.Skip(1).Reverse().Concat(lpw).ToList();
        }
        private List<double> Hpf(double fc, int m)
        {
            var lpw = LPFPorter(fc, m);
            int l = 2 * m + 1;
            return lpw
                .Select((v, i) => i == m ? 1 - v : -1 * v)
                .ToList();
        }

        private List<double> Bpf(double f1, double f2, int m)
        {
            var lpw1 = LPFPorter(f1, m);
            var lpw2 = LPFPorter(f2, m);
            return lpw1.Select((v, i) => lpw2[i] - v).ToList();
        }


        private List<double> Bsf(double f1, double f2, int m)
        {
            var lpw1 = LPFPorter(f1, m);
            var lpw2 = LPFPorter(f2, m);
            return lpw1.Select((v, i) => v - lpw2[i] + (i == m ? 1 : 0)).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int N = 20;
            double a = 5, b = 3;
            List<double> answer1 = Linear(N, a, b);
            N = 20;
            a = -5; b = 3;
            List<double> answer2 = Linear(N, a, b);
            N = 20;
            a = 0.2; b = 1;
            List<double> answer3 = Exponential(N, a, b);
            N = 20;
            a = -0.2; b = 1;
            List<double> answer4 = Exponential(N, a, b);
            this.chart1.Series[0].Points.Clear();
            this.chart2.Series[0].Points.Clear();
            this.chart3.Series[0].Points.Clear();
            this.chart4.Series[0].Points.Clear();
            for (int i = 0; i < answer1.Count(); i++)
                this.chart1.Series[0].Points.AddXY(i, answer1[i]);
            for (int i = 0; i < answer2.Count(); i++)
                this.chart2.Series[0].Points.AddXY(i, answer2[i]);
            for (int i = 0; i < answer3.Count(); i++)
                this.chart3.Series[0].Points.AddXY(i, answer3[i]);
            for (int i = 0; i < answer4.Count(); i++)
                this.chart4.Series[0].Points.AddXY(i, answer4[i]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<double> answer1 = EmbedRandom();
            List<double> answer2 = SelfRandom();
            this.chart5.Series[0].Points.Clear();
            this.chart6.Series[0].Points.Clear();
            for (int i = 0; i < answer1.Count(); i++)
                this.chart5.Series[0].Points.AddXY(i, answer1[i]);
            for (int i = 0; i < answer2.Count(); i++)
                this.chart6.Series[0].Points.AddXY(i, answer2[i]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Harmonic();
            List<double> answer1 = EmbedRandom();
            this.chart7.Series[0].Points.Clear();
            for (int i = 0; i < answer1.Count(); i++)
                this.chart7.Series[0].Points.AddXY(i, answer1[i]);
            if (stationarity())
            {
                MessageBox.Show("Стационарный");
            }
            else
            {
                MessageBox.Show("Не стационарный");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<double> answer1 = EmbedRandom();
            this.chart19.Series[0].Points.Clear();
            for (int i = 0; i < answer1.Count(); i++)
                this.chart19.Series[0].Points.AddXY(i, answer1[i]);
            List<double> answer2 = EmbedRandom();
            this.chart20.Series[0].Points.Clear();
            for (int i = 0; i < answer2.Count(); i++)
                this.chart20.Series[0].Points.AddXY(i, answer2[i]);
            List<double> x1 = EmbedRandom();
            System.Threading.Thread.Sleep(50);
            List<double> y = EmbedRandom();
            List<double> L = covariance(x1, y);
            double Num1 = L.Count();
            this.chart10.Series[0].Points.Clear();
            for (int t = 0; t < Num1; t++)
            {
                this.chart10.Series[0].Points.AddXY(t, L[t]);
            }
            List<double> x = EmbedRandom();
            List<double> Y = avtocovariance(x);
            double Num = Y.Count();
            this.chart12.Series[0].Points.Clear();
            for (int t = 0; t < Num; t++)
            {
                textBox21.Text += Y[t].ToString() + " / ";
                this.chart12.Series[0].Points.AddXY(t, Y[t]);
            }
            density(EmbedRandom(), ref this.chart11);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<double> answer1 = Harm();
            this.chart14.Series[0].Points.Clear();
            for (int i = 0; i < answer1.Count(); i++)
                this.chart14.Series[0].Points.AddXY(i, answer1[i]);
            List<double> answer2 = polyHarm3();
            this.chart15.Series[0].Points.Clear();
            for (int i = 0; i < answer2.Count(); i++)
                this.chart15.Series[0].Points.AddXY(i, answer2[i]);
            List<double> x = Harm();
            List<double> answer3 = avtocovariance(x);
            this.chart16.Series[0].Points.Clear();
            for (int i = 0; i < answer3.Count(); i++)
                this.chart16.Series[0].Points.AddXY(i, answer3[i]);
            List<double> x1 = Harm();
            List<double> y = polyHarm3();
            List<double> answer4 = covariance(x1, y);
            this.chart17.Series[0].Points.Clear();
            for (int i = 0; i < answer4.Count(); i++)
                this.chart17.Series[0].Points.AddXY(i, answer4[i]);
            List<double> x2 = polyHarm3();
            List<double> answer5 = avtocovariance(x2);
            this.chart13.Series[0].Points.Clear();
            for (int i = 0; i < answer5.Count(); i++)
                this.chart13.Series[0].Points.AddXY(i, answer5[i]);
            density(Harm(), ref this.chart18);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.chart22.Series[0].Points.Clear();
            this.chart21.Series[0].Points.Clear();
            List<double> x = Harm();
            var tmp = Shift(x);
            for (int i = 0; i < tmp.Count(); i++)
                this.chart22.Series[0].Points.AddXY(i, tmp[i]);
            tmp = spikes(tmp);
            for (int i = 0; i < tmp.Count(); i++)
                this.chart21.Series[0].Points.AddXY(i, tmp[i]);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.chart23.Series[0].Points.Clear();
            List<double> answer1 = Harm();
            var tmp = antishift(answer1);
            for (int i = 0; i < tmp.Count(); i++)
                this.chart23.Series[0].Points.AddXY(i, tmp[i]);

            this.chart24.Series[0].Points.Clear();
            List<double> answer2 = Harm();
            var tmp2 = antispike(answer2);
            for (int i = 0; i < tmp2.Count(); i++)
                this.chart24.Series[0].Points.AddXY(i, tmp2[i]);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.chart25.Series[0].Points.Clear();
            int N = 1000;
            double a = 4, b = 5;
            List<double> answer1 = Linear(N, a, b);
            var tmp = additiveTrRand(answer1);
            for (int i = 0; i < tmp.Count(); i++)
                this.chart25.Series[0].Points.AddXY(i, tmp[i]);
            this.chart26.Series[0].Points.Clear();
            var tmp2 = HighlightingTrend(answer1);
            for (int i = 0; i < tmp2.Count(); i++)
                this.chart26.Series[0].Points.AddXY(i, tmp2[i]);
            this.chart27.Series[0].Points.Clear();
            for (int i = 0; i < tmp2.Count; i++) // antiTrend
            {
                var tmp3 = tmp[i] - tmp2[i];
                this.chart27.Series[0].Points.AddXY(i, tmp3);
            }
            this.chart29.Series[0].Points.Clear();
            List<double> answer2 = EmbedRandom();
            tmp2 = HarmZad3(answer2);
            for (int i = 0; i < tmp2.Count(); i++)
                this.chart29.Series[0].Points.AddXY(i, tmp2[i]);
            AnalyticalError(answer2);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int N2 = 1000;
            double dt = 0.002;
            double dk = 1 / (2 * dt);
            double df = dk / (N2 / 2);
            this.chart28.Series[0].Points.Clear();
            List<double> tmp = Harm();
            for (int i = 1; i < tmp.Count(); i++)
                this.chart28.Series[0].Points.AddXY(i, tmp[i]);
            this.chart30.Series[0].Points.Clear();
            chart30.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart30.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            var tmp2 = FourierAmplitude(tmp);
            // tmp2 = Shift(tmp2);
            for (int i = 1; i < tmp2.Count(); i++)
            {
                this.chart30.Series[0].Points.AddXY(i * df, tmp2[i]);
                Console.WriteLine(chart30.Series[0].Points[i - 1]);
            }
            this.chart31.Series[0].Points.Clear();
            double window = 0.91;
            var tmp3 = FourierSpectrum(tmp, window);
            for (int i = 1; i < tmp3.Count(); i++)
                this.chart31.Series[0].Points.AddXY(i * df, tmp3[i]);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int N = 1000;
            int N2 = 1000;
            float[] p = ReadFile();
            this.chart32.Series[0].Points.Clear();
            for (int i = 1; i < p.Count(); i++)
                this.chart32.Series[0].Points.AddXY(i, p[i]);
            List<double> lis = p.ToList().Select(x => Convert.ToDouble(x)).ToList();
            var arr2 = FourierAmplitude(lis);
            arr2 = Shift(arr2);
            double dt = 0.02;
            double dk = 1 / (2 * dt);
            double df = dk / (N2 / 2);
            this.chart33.Series[0].Points.Clear();
            for (int i = 1; i < arr2.Count(); i++)
            {
                this.chart33.Series[0].Points.AddXY(i * df, arr2[i]);
                Console.WriteLine(chart33.Series[0].Points[i - 1]);

            }
            double window = 0.91;
            var arr3 = FourierSpectrum(lis, window);
            this.chart34.Series[0].Points.Clear();
            for (int i = 1; i < arr3.Count(); i++)
                this.chart34.Series[0].Points.AddXY(i * df, arr3[i]);
        }

        private static float[] ReadFile()
        {
            string path = @"C:\Users\Nikita\Desktop\Maga 1 sem\Experimental data processing methods\DataExperimental-master\v1x8.dat";
            byte[] arr = File.ReadAllBytes(path);
            float[] p = new float[arr.Length / 4];
            for (int i = 0; i < arr.Length / 4; i++)
            {
                p[i] = BitConverter.ToSingle(arr, i * 4);
            }

            return p;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int N = 1000; // количество значений в входном сигнале
            int M = 200; // количество значений в функции h
            int start = 1; // начальное значение
            int step = 1; // шаг по умолчанию
            double dt = 0.005; // шаг дискретизации
            double A = 10, f = 4; // амплитуда гармонического процесса, частота
            // параметры экспоненты
            double a = 10, b = 1;
            List<double> x = new List<double>();
            chart35.Series[0].Points.Clear();
            for (int i = start; i < N; i += step)
            {
                int del = 0;
                switch (i)
                {
                    case 200: del = 120; break;
                    case 400: del = 130; break;
                    case 600: del = 110; break;
                }
                x.Add(del);
                chart35.Series[0].Points.AddXY(i, del);
            }

            List<double> h = new List<double>();
            chart36.Series[0].Points.Clear();
            for (double i = start; i < M * dt + 1; i += dt)
            {
                // (exp * sin)
                h.Add((Math.Exp(-a * i) * b) * (A * Math.Sin(2 * Math.PI * f * i)));
            }
            double m = h.Max();
            double j = 0;
            for (int i = 0; i < h.Count(); i++)
            {
                h[i] /= m;
                chart36.Series[0].Points.AddXY(j, h[i]);
                j += Math.Round(1.0 / h.Count(), 3);
            }
            chart37.Series[0].Points.Clear();
            List<double> card = new List<double>();
            for (int i = 0; i < N + M; i++)
            {
                double sum = 0;
                for (int k = 0; k < M; k++)
                {
                    if (i - k > 0 && i - k < x.Count())
                        sum += x[i - k] * h[k];
                }
                card.Add(sum);
            }
            for (int i = M / 2, l = 1; i < card.Count() - M / 2; i++)
            {
                chart37.Series[0].Points.AddXY(l++, card[i]);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            double dt = 0.002;
            int m = 128;
            this.chart38.Series[0].Points.Clear();
            List<double> tmp = LPFPorter(10 * dt, m);
            for (int i = 1; i < tmp.Count(); i++)
                this.chart38.Series[0].Points.AddXY(i, tmp[i]);
            this.chart39.Series[0].Points.Clear();
            tmp = Hpf(90 * dt, m);
            for (int i = 1; i < tmp.Count(); i++)
                this.chart39.Series[0].Points.AddXY(i, tmp[i]);
            this.chart40.Series[0].Points.Clear();
            tmp = Bpf(10 * dt, 50 * dt, m);
            for (int i = 1; i < tmp.Count(); i++)
                this.chart40.Series[0].Points.AddXY(i, tmp[i]);
            this.chart41.Series[0].Points.Clear();
            tmp = Bsf(10 * dt, 50 * dt, m);
            for (int i = 1; i < tmp.Count(); i++)
                this.chart41.Series[0].Points.AddXY(i, tmp[i]);
        }

        private List<double> Convolution(List<double> a, List<double> b)
        {
            var res = new List<double>();
            for (int k = 0; k < a.Count + b.Count; k++)
            {
                if (k >= a.Count / 2 && k < b.Count + a.Count / 2)
                {
                    double sum = 0;
                    for (int j = 0; j < b.Count; j++)
                    {
                        if (k - j < 0)
                            break;
                        if (k - j < a.Count)
                            sum += a[k - j] * b[j];
                    }
                    res.Add(sum);
                }
            }
            return res;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            double dt = 0.002;
            double m = 128;
            chart45.Series[0].Points.Clear();
            List<double> tmp = FourierSpectrum(LPFPorter(10 * dt, (int)m), 1);
            /// 0, (2m+1)df/2, df
            /// y[0..len / 2]
            /// (int)Math.Ceil(((2m+1)df/2) / df)
            double x = 0.0, df = 1 / ((2 * m + 1) * dt);
            for (int i = 0; i < (int)((2 * m + 1) / 2); i++)
            {
                this.chart45.Series[0].Points.AddXY(x, tmp[i]);
                x += df;
            }
            this.chart44.Series[0].Points.Clear();
            tmp = FourierSpectrum(Hpf(90 * dt, (int)m), 1);
            x = 0.0; df = 1 / ((2 * m + 1) * dt);
            for (int i = 1; i < (int)((2 * m + 1) / 2); i++)
            {
                this.chart44.Series[0].Points.AddXY(x, tmp[i]);
                x += df;
            }
            this.chart43.Series[0].Points.Clear();
            tmp = FourierSpectrum(Bpf(10 * dt, 50 * dt, (int)m), 1);
            x = 0.0; df = 1 / ((2 * m + 1) * dt);
            for (int i = 1; i < (int)((2 * m + 1) / 2); i++)
            {
                this.chart43.Series[0].Points.AddXY(x, tmp[i]);
                x += df;
            }
            this.chart42.Series[0].Points.Clear();
            tmp = FourierSpectrum(Bsf(10 * dt, 50 * dt, (int)m), 1);
            x = 0.0; df = 1 / ((2 * m + 1) * dt);
            for (int i = 1; i < (int)((2 * m + 1) / 2); i++)
            {
                this.chart42.Series[0].Points.AddXY(x, tmp[i]);
                x += df;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            var fl = ReadFile().Select(v => Convert.ToDouble(v)).ToList();
            double dt = 0.002;
            int m = 128;
            this.chart49.Series[0].Points.Clear();
            List<double> tmp = Convolution(fl, LPFPorter(10 * dt, m));
            double x = 0;
            for (int i = 0; i < tmp.Count(); i++)
            {
                this.chart49.Series[0].Points.AddXY(x, tmp[i]);
                x += dt;
            }
            this.chart48.Series[0].Points.Clear();
            tmp = Convolution(fl, Hpf(90 * dt, m));
            x = 0;
            for (int i = 1; i < tmp.Count(); i++)
            {
                this.chart48.Series[0].Points.AddXY(x, tmp[i]);
                x += dt;
            }
            this.chart47.Series[0].Points.Clear();
            tmp = Convolution(fl, Bpf(10 * dt, 50 * dt, m));
            x = 0;
            for (int i = 1; i < tmp.Count(); i++)
            {
                this.chart47.Series[0].Points.AddXY(x, tmp[i]);
                x += dt;
            }
            this.chart46.Series[0].Points.Clear();
            tmp = Convolution(fl, Bsf(10 * dt, 50 * dt, m));
            x = 0;
            for (int i = 1; i < tmp.Count(); i++)
            {
                this.chart46.Series[0].Points.AddXY(x, tmp[i]);
                x += dt;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            var fl = ReadFile().Select(v => Convert.ToDouble(v)).ToList();
            double dt = 0.002;
            double m = 128;
            chart53.Series[0].Points.Clear();
            List<double> tmp = FourierSpectrum(Convolution(fl, LPFPorter(10 * dt, (int)m)), 1);
            /// 0, (2m+1)df/2, df
            /// y[0..len / 2]
            /// (int)Math.Ceil(((2m+1)df/2) / df)
            double x = 0.0, df = 1 / ((2 * m + 1) * dt);
            for (int i = 0; i < (int)((2 * m + 1) / 2); i++)
            {
                this.chart53.Series[0].Points.AddXY(x, tmp[i]);
                x += df;
            }
            this.chart52.Series[0].Points.Clear();
            tmp = FourierSpectrum(Convolution(fl, Hpf(90 * dt, (int)m)), 1);
            x = 0.0; df = 1 / ((2 * m + 1) * dt);
            for (int i = 1; i < (int)((2 * m + 1) / 2); i++)
            {
                this.chart52.Series[0].Points.AddXY(x, tmp[i]);
                x += df;
            }
            this.chart51.Series[0].Points.Clear();
            tmp = FourierSpectrum(Convolution(fl, Bpf(10 * dt, 50 * dt, (int)m)), 1);
            x = 0.0; df = 1 / ((2 * m + 1) * dt);
            for (int i = 1; i < (int)((2 * m + 1) / 2); i++)
            {
                this.chart51.Series[0].Points.AddXY(x, tmp[i]);
                x += df;
            }
            this.chart50.Series[0].Points.Clear();
            tmp = FourierSpectrum(Convolution(fl, Bsf(10 * dt, 50 * dt, (int)m)), 1);
            x = 0.0; df = 1 / ((2 * m + 1) * dt);
            for (int i = 1; i < (int)((2 * m + 1) / 2); i++)
            {
                this.chart50.Series[0].Points.AddXY(x, tmp[i]);
                x += df;
            }
        }

        private static double[] OpenWavFile(string path)
        {
            double[] left;
            double[] right;

            byte[] wav = File.ReadAllBytes(path);
            int channels = wav[22];

            int pos = 12;

            Func<byte, byte, double> BytesToDouble = (byte firstByte, byte secondByte) => ((short)((secondByte << 8) | firstByte)) / 32768.0;

            while (!(wav[pos] == 100 && wav[pos + 1] == 97 && wav[pos + 2] == 116 && wav[pos + 3] == 97))
            {
                pos += 4;
                int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
                pos += 4 + chunkSize;
            }
            pos += 8;

            int samples = (wav.Length - pos) / 2;
            if (channels == 2) samples /= 2;

            left = new double[samples];
            if (channels == 2) right = new double[samples];
            else right = null;


            int i = 0;
            while (pos < wav.Length)
            {
                left[i] = BytesToDouble(wav[pos], wav[pos + 1]);
                pos += 2;
                if (channels == 2)
                {
                    right[i] = BytesToDouble(wav[pos], wav[pos + 1]);
                    pos += 2;
                }
                i++;
            }

            return left;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            chart54.Series[0].Points.Clear();
            chart55.Series[0].Points.Clear();
            Cursor.Current = Cursors.WaitCursor;

            string path;

            using (OpenFileDialog op = new OpenFileDialog())
            {
                if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    path = op.FileName;
                }
                else
                {
                    return;
                }
            }

            double[] wavNach = OpenWavFile(path);
            double[] wav = new double[22103];
            int len = wavNach.Length;
            for (int i = 0; i < 22103; i++)
            {
                wav[i] = wavNach[i + 16000];
            }
            for (int i = 0; i < wav.Length; i++)
            {
                chart54.Series[0].Points.AddXY(i, wav[i]);
            }
            List<double> fur = FourierAmplitude(new List<double>(wav));
            chart55.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            for (int i = 0; i < fur.Count / 2; i++)
            {
                chart55.Series[0].Points.AddXY(i, fur[i]);
            }
            wav = wav.Select(i => i * 10).ToArray();
            float[] floatArray = wav.Select(s => (float)s).ToArray();
            using (WaveFileWriter writer = new WaveFileWriter("10och.wav", new WaveFormat(22050, 16, 1)))
            {
                writer.WriteSamples(floatArray, 0, floatArray.Length);
            }
            Cursor.Current = Cursors.Default;
        }

        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {
            double dt = 1.0 / 22050.0;
            chart60.Series[0].Points.Clear();
            chart62.Series[0].Points.Clear();
            string txt = ((Button)sender).Text;
            ((Button)sender).Text = "Wait";
            chart60.Titles[0].Text = "Исходный звук";
            chart62.Titles[0].Text = "Разложение Фурье";

            string path;

            using (OpenFileDialog op = new OpenFileDialog())
            {
                if (op.ShowDialog() == DialogResult.OK)
                {
                    path = op.FileName;
                }
                else
                {
                    return;
                }
            }

            double[] wavNach = OpenWavFile(path);
            wav = wavNach.ToList();
            chart60.Series[0].Points.DataBindXY(Enumerable.Range(0, wav.Count).Select(i => i * dt).ToArray(), wav);
            System.Threading.Thread.Sleep(1000);
            var fur = FourierAmplitude(wav);
            chart62.Series[0].Points.DataBindXY(Enumerable.Range(0, 1000).ToArray(), fur.Take(1000).ToArray());
           
            Cursor.Current = Cursors.Default;
            ((Button)sender).Text = txt;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            int m = 128;
            double dt = 1.0 / 22050.0;
            chart60.Series[0].Points.Clear();
            chart62.Series[0].Points.Clear();
            chart60.Titles[0].Text = "Основной тон";
            string txt = ((Button)sender).Text;
            ((Button)sender).Text = "Wait";
            System.Threading.Thread.Sleep(1000);
            var lpf = Convolution(LPFPorter(200.0 * dt, m), wav);
            float[] floatArray = lpf.Select(s => (float)(s * 5.0)).ToArray();
            using (WaveFileWriter writer = new WaveFileWriter("Основной.wav", new WaveFormat(22050, 16, 1)))
            {
                writer.WriteSamples(floatArray, 0, floatArray.Length);
            }
            double time = 0;
            chart60.Series[0].Points.DataBindXY(Enumerable.Range(0, lpf.Count).Select(i => i * dt).ToArray(), lpf);
            var fur = FourierAmplitude(lpf);
            chart62.Series[0].Points.DataBindXY(Enumerable.Range(0, 1000).ToArray(), fur.Take(1000).ToArray());

            Cursor.Current = Cursors.Default;
            ((Button)sender).Text = txt;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            int m = 128;
            double dt = 1.0 / 22050.0;
            chart60.Series[0].Points.Clear();
            chart62.Series[0].Points.Clear();
            string txt = ((Button)sender).Text;
            ((Button)sender).Text = "Wait";
            System.Threading.Thread.Sleep(1000);
            var bpf = Convolution(Bpf(250.0 * dt, 350.0 * dt, m), wav);
            double time = 0;
            for (int i = 0; i < bpf.Count; i++)
            {
                chart60.Series[0].Points.AddXY(time, bpf[i]);
                time += dt;
            }
            float[] floatArray = bpf.Select(s => (float)(s * 5.0)).ToArray();
            using (WaveFileWriter writer = new WaveFileWriter("1for.wav", new WaveFormat(22050, 16, 1)))
            {
                writer.WriteSamples(floatArray, 0, floatArray.Length);
            }
            var fur = FourierAmplitude(bpf);
            for (int i = 0; i < fur.Count / 2 && i < 1000; i++)
            {
                chart62.Series[0].Points.AddXY(i, fur[i]);
            }
            Cursor.Current = Cursors.Default;
            ((Button)sender).Text = txt;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            int m = 128;
            double dt = 1.0 / 22050.0;
            chart60.Series[0].Points.Clear();
            chart62.Series[0].Points.Clear();
            string txt = ((Button)sender).Text;
            ((Button)sender).Text = "Wait";
            System.Threading.Thread.Sleep(1000);
            var bpf = Convolution(Bpf(450 * dt, 600 * dt, m), wav);
            double time = 0;
            for (int i = 0; i < bpf.Count; i++)
            {
                chart60.Series[0].Points.AddXY(time, bpf[i]);
                time += dt;
            }
            float[] floatArray = bpf.Select(s => (float)(s * 10.0)).ToArray();
            using (WaveFileWriter writer = new WaveFileWriter("2for.wav", new WaveFormat(22050, 16, 1)))
            {
                writer.WriteSamples(floatArray, 0, floatArray.Length);
            }
            var fur = FourierAmplitude(bpf);
            for (int i = 0; i < fur.Count / 2 && i < 1000; i++)
            {
                chart62.Series[0].Points.AddXY(i, fur[i]);
            }
            Cursor.Current = Cursors.Default;
            ((Button)sender).Text = txt;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            int m = 128;
            double dt = 1.0 / 22050.0;
            chart60.Series[0].Points.Clear();
            chart62.Series[0].Points.Clear();
            string txt = ((Button)sender).Text;
            ((Button)sender).Text = "Wait";
            System.Threading.Thread.Sleep(1000);
            var bpf = Convolution(Bpf(650 * dt, 750 * dt, m), wav);
            double time = 0;
            for (int i = 0; i < bpf.Count; i++)
            {
                chart60.Series[0].Points.AddXY(time, bpf[i]);
                time += dt;
            }
            float[] floatArray = bpf.Select(s => (float)(s * 20.0)).ToArray();
            using (WaveFileWriter writer = new WaveFileWriter("3for.wav", new WaveFormat(22050, 16, 1)))
            {
                writer.WriteSamples(floatArray, 0, floatArray.Length);
            }
            var fur = FourierAmplitude(bpf);
            for (int i = 0; i < fur.Count / 2 && i < 1000; i++)
            {
                chart62.Series[0].Points.AddXY(i, fur[i]);
            }
            Cursor.Current = Cursors.Default;
            ((Button)sender).Text = txt;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            int m = 128;
            double dt = 1.0 / 22050.0;
            chart60.Series[0].Points.Clear();
            chart62.Series[0].Points.Clear();
            string txt = ((Button)sender).Text;
            ((Button)sender).Text = "Wait";
            System.Threading.Thread.Sleep(1000);
            var bpf = Convolution(Bpf(800 * dt, 900 * dt, m), wav);
            double time = 0;
            for (int i = 0; i < bpf.Count; i++)
            {
                chart60.Series[0].Points.AddXY(time, bpf[i]);
                time += dt;
            }
            float[] floatArray = bpf.Select(s => (float)(s * 100.0)).ToArray();
            using (WaveFileWriter writer = new WaveFileWriter("4for.wav", new WaveFormat(22050, 16, 1)))
            {
                writer.WriteSamples(floatArray, 0, floatArray.Length);
            }
            var fur = FourierAmplitude(bpf);
            for (int i = 0; i < fur.Count / 2 && i < 1000; i++)
            {
                chart62.Series[0].Points.AddXY(i, fur[i]);
            }
            Cursor.Current = Cursors.Default;
            ((Button)sender).Text = txt;
        }
    }
}
