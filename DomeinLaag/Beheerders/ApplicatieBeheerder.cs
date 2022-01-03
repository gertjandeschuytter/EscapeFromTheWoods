using DataLaag.ADO;
using DomeinLaag.Exceptions;
using DomeinLaag.Interfaces;
using DomeinLaag.Klassen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace DomeinLaag.Beheerders {
    public static class ApplicatieBeheerder {

        #region Methods
        public static void GenereerDirectories()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path += @"\EscapeFromTheWoods";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                Directory.Delete(path, true);
            }
            string path1 = path + @"\Bitmappen";
            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }
            string path2 = path + @"\Tekstbestanden";
            if (!Directory.Exists(path2))
            {
                Directory.CreateDirectory(path2);
            }
        }
        private static async Task GenereerBitmap(Bos bos, List<Boom> bomen, List<Aap> apen)
        {
            try
            {
                Console.WriteLine($"Start bitmap - bos{bos.Id}");
                Random random = new();
                Bitmap bitmap = new(bos.Xmax - bos.Xmin, bos.Ymax - bos.Ymin);
                bitmap.SetResolution(150.0F, 150.0F);

                Graphics graphics = Graphics.FromImage(bitmap);
                Pen penGroen = new(Color.DarkGreen, 1);

                graphics.Clear(Color.Black);

                foreach (Boom boom in bomen)
                {
                    graphics.DrawEllipse(penGroen, boom.X - 3, boom.Y - 3, 6, 6);
                }

                foreach (Aap aap in apen)
                {
                    int rood = random.Next(50, 256);
                    int groen = random.Next(50, 256);
                    int blauw = random.Next(50, 256);

                    SolidBrush brush = new(Color.FromArgb(rood, groen, blauw));
                    Pen pen = new(Color.FromArgb(rood, groen, blauw));

                    graphics.FillEllipse(brush, aap.Bomen[0].X - 3, aap.Bomen[0].Y - 3, 6, 6);
                    for (int i = 0; i < aap.Bomen.Count - 1; i++)
                    {
                        if (aap.Bomen[i + 1].X != -1)
                        {
                            graphics.DrawLine(pen, aap.Bomen[i].X, aap.Bomen[i].Y, aap.Bomen[i + 1].X, aap.Bomen[i + 1].Y);
                        }
                    }
                }

                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                path += $"\\EscapeFromTheWoods\\Bitmappen";
                bitmap.Save(Path.Combine(path, $"Bos{bos.Id}_EscapeRoute.jpg"), ImageFormat.Jpeg);
                Console.WriteLine($"Einde bitmap - bos{bos.Id}");
            }
            catch (Exception ex)
            {
                throw new ApplicatieBeheerderException("Kan geen bitmap genereren.", ex);
            }
        }
#pragma warning restore CA1416 // Validate platform compatibility

        private static async Task GenereerTekstBestand(Bos bos, List<Aap> apen)
        {
            try
            {
                Console.WriteLine($"Start tekstbestand - bos{bos.Id}");
                int langsteNaam = 0;
                foreach (Aap aap in apen)
                {
                    if (aap.Naam.Length > langsteNaam)
                    {
                        langsteNaam = aap.Naam.Length;
                    }
                }

                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                path += $"\\EscapeFromTheWoods\\Tekstbestanden\\Bos{bos.Id}.txt";
                await using StreamWriter streamWriter = File.CreateText(path);

                int aantalStappen = 0;
                foreach (Aap aap in apen)
                {
                    if (aantalStappen < aap.Bomen.Count)
                    {
                        aantalStappen = aap.Bomen.Count;
                    }
                }

                int ontsnapt = 0;
                for (int i = 0; i < aantalStappen; i++)
                {
                    foreach (Aap aap in apen)
                    {
                        if (aap.Bomen.Count > i)
                        {
                            if (aap.Bomen[i].X != -1)
                            {
                                await Task.Run(() => streamWriter.WriteLine($"{aap.Naam.PadRight(langsteNaam, ' ')} zit in een boom met Id: {aap.Bomen[i].Id,3} met de coördinaten ({aap.Bomen[i].X,3}, {aap.Bomen[i].Y,3})."));
                            }
                            else
                            {
                                switch (ontsnapt)
                                {
                                    case 0:
                                    await Task.Run(() => streamWriter.WriteLine($"-----------------------------------------------------------------\n" +
                                $"Gefeliciteerd {aap.Naam}, je bent als eerste uit het bos geraakt.\n" +
                                $"-----------------------------------------------------------------"));
                                    break;
                                    case 1:
                                    await Task.Run(() => streamWriter.WriteLine($"-----------------------------------------------------------------\n" +
                                $"Gefeliciteerd {aap.Naam}, je bent als tweeded uit het bos geraakt.\n" +
                                $"-----------------------------------------------------------------"));
                                    break;
                                    case 2:
                                    await Task.Run(() => streamWriter.WriteLine($"-----------------------------------------------------------------\n" +
                                $"Gefeliciteerd {aap.Naam}, je bent als derde uit het bos geraakt.\n" +
                                $"-----------------------------------------------------------------"));
                                    break;
                                    default:
                                    await Task.Run(() => streamWriter.WriteLine($"-----------------------------------------------------------------\n" +
                                $"Gefeliciteerd {aap.Naam}, je bent uit het bos geraakt.\n" +
                                $"-----------------------------------------------------------------"));
                                    break;
                                }
                                ontsnapt++;
                            }
                        }
                    }
                }
                Console.WriteLine($"Stop tekstbestand - bos{bos.Id}");
            }
            catch (Exception ex)
            {
                throw new ApplicatieBeheerderException("Kan geen tekstbestand genereren.", ex);
            }
        }
        public static async Task DataUploadenNaarDatabank(Bos bos, List<Boom> bomen, List<Aap> apen)
        {
            try
            {
                await VoegBoomToe(bomen, bos);
                await VoegAapGegevens(apen, bos);
                await VoegLogGegevens(apen, bos);

            }
            catch (Exception ex)
            {
                throw new ApplicatieBeheerderException("Kan niet uploaden naar databank", ex);
            }
        }
        private static async Task VoegAapGegevens(List<Aap> apen, Bos bos)
        {
            SqlConnection connection = ConnectionManager.GetConnection();
            string query = "INSERT INTO dbo.AapRecords (BosId, Naam, BosRecordsId, BoomId, SequencieNummer, X,Y) VALUES(@BosId, @Naam, @BosRecordsId, @boomID, @SequencieNummer, @X, @Y)";
            foreach (Aap element in apen)
            {
                int seqnr = 0;
                foreach (Boom el in element.Bomen)
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        await Task.Run(() => connection.Open());
                        try
                        {
                            Console.WriteLine("data aan het uploaden");
                            command.Parameters.AddWithValue("@BosId", SqlDbType.Int).Value = element.Id;
                            command.Parameters.AddWithValue("@Naam", SqlDbType.Int).Value = element.Naam;
                            command.Parameters.AddWithValue("@BosRecordsId", SqlDbType.Int).Value = bos.Id;
                            command.Parameters.AddWithValue("@boomID", SqlDbType.Int).Value = el.Id;
                            command.Parameters.AddWithValue("@SequencieNummer", SqlDbType.Int).Value = seqnr;
                            command.Parameters.AddWithValue("@X", SqlDbType.Int).Value = el.X;
                            command.Parameters.AddWithValue("@Y", SqlDbType.Int).Value = el.Y;
                            command.CommandText = query;
                            command.ExecuteNonQuery();
                            seqnr++;
                            Console.WriteLine("Gestopt met data aan het uploaden");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
        }
        private static async Task VoegLogGegevens(List<Aap> apen, Bos bos)
        {
            SqlConnection connection = ConnectionManager.GetConnection();
            string query = "INSERT INTO dbo.Logs (BosId, AapId, Bericht) VALUES(@BosId, @AapId, @Bericht)";
            foreach (Aap element in apen)
            {
                int seqnr = 0;
                foreach (Boom el in element.Bomen)
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        await Task.Run(() => connection.Open());
                        try
                        {
                            Console.WriteLine("data aan het uploaden");
                            command.Parameters.AddWithValue("@AapId", SqlDbType.Int).Value = element.Id;
                            command.Parameters.AddWithValue("@BosId", SqlDbType.Int).Value = bos.Id;
                            if (el.Y == -1)
                                command.Parameters.AddWithValue("@Bericht", SqlDbType.NChar).Value = element.Naam + " is uit het bos";
                            else
                                command.Parameters.AddWithValue("@Bericht", SqlDbType.NChar).Value = element.Naam + " zit nu in boom " + el.Id + " op locatie (" + el.X + "," + el.Y + ")";

                            command.CommandText = query;
                            command.ExecuteNonQuery();
                            seqnr++;
                            Console.WriteLine("Gestopt met data aan het uploaden");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
        }
        private static async Task VoegBoomToe(List<Boom> bomen, Bos bos)
        {
            SqlConnection connection = ConnectionManager.GetConnection();
            string query = "INSERT INTO dbo.BosRecords (BosId, BoomId, X, Y) VALUES(@BosId, @BoomId, @X, @Y)";
            foreach (Boom element in bomen)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    await Task.Run(() => connection.Open());
                    try
                    {
                        Console.WriteLine("Data aan het uploaden");
                        command.Parameters.AddWithValue("@BosId", SqlDbType.Int).Value = bos.Id;
                        command.Parameters.AddWithValue("@BoomId", SqlDbType.Int).Value = element.Id;
                        command.Parameters.AddWithValue("@X", SqlDbType.Int).Value = element.X;
                        command.Parameters.AddWithValue("@Y", SqlDbType.Int).Value = element.Y;
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                        Console.WriteLine("Gestopt met data aan het uploaden");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private static async Task ProcessData(Bos bos)
        {
            List<Task> tasks1 = new();
            tasks1.Add(Task.Run(() => GenereerBitmap(bos, bos.Bomen, bos.Apen)));
            tasks1.Add(Task.Run(() => GenereerTekstBestand(bos, bos.Apen)));
            tasks1.Add(Task.Run(() => DataUploadenNaarDatabank(bos, bos.Bomen, bos.Apen)));
            Task.WaitAll(tasks1.ToArray());

        }
        public static async Task Process(Bos bos)
        {
            await AapBeheerder.ProcessAap(bos);
            await ProcessData(bos);
        }
        #endregion
    }
}
