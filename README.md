# Tubes_Terserah

Tugas Besar Strategi Algoritma Robocode Tank Royale

# Greedy Algorithm Bot for Robocode Tank Royale

## Deskripsi

Robocode Tank Royale adalah permainan pemrograman robot tank virtual, di mana setiap pemain membuat bot yang dapat bergerak, memindai lawan, menembak, dan bertahan di dalam arena pertandingan. Pada tugas besar ini, kami mengimplementasikan beberapa strategi berbasis algoritma greedy menggunakan bahasa pemrograman C#.

Algoritma greedy digunakan karena bot harus mengambil keputusan secara cepat berdasarkan kondisi saat ini. Setiap bot lawan yang terdeteksi radar dianggap sebagai kandidat, kemudian bot memilih aksi terbaik berdasarkan kriteria tertentu, seperti jarak terdekat, energi lawan terendah, risiko bertahan hidup, atau skor peluang target.

## Algoritma Greedy yang Diimplementasikan

1. **GreedyClosestBot**  
   Bot ini menggunakan strategi **Greedy by Distance**, yaitu memilih musuh dengan jarak paling dekat sebagai target utama. Strategi ini didasarkan pada asumsi bahwa semakin dekat posisi lawan, semakin besar peluang peluru mengenai target. Setelah target terdekat ditemukan, bot akan mengarahkan senjata ke posisi target dan menembak ketika gun sudah siap.

2. **GreedyEnergyBot**  
   Bot ini menggunakan strategi **Greedy by Lowest Energy**, yaitu memilih musuh dengan energi paling rendah. Strategi ini bertujuan untuk meningkatkan peluang eliminasi terhadap lawan yang sudah lemah. Jika terdapat dua musuh dengan energi yang sama, bot akan memilih musuh yang jaraknya lebih dekat.

3. **GreedySurvivalBot**  
   Bot ini menggunakan strategi **Greedy by Survival Risk**, yaitu memilih musuh terdekat sebagai ancaman utama. Fokus bot ini bukan hanya menyerang, tetapi juga menjaga jarak aman agar tidak mudah terkena tembakan atau tabrakan. Bot akan bergerak menjauh ketika musuh terlalu dekat dan bergerak menyamping ketika musuh berada pada jarak sedang.

4. **GreedyMainBot**  
   Bot utama menggunakan strategi **Greedy by Opportunity Score**, yaitu memilih target berdasarkan skor gabungan dari jarak musuh, energi musuh, dan kelayakan jarak tembak. Target dengan skor tertinggi dipilih sebagai target utama pada kondisi saat itu.

   Rumus evaluasi yang digunakan adalah:

   ```text
   Score(target) = 0.45 × DistanceScore + 0.35 × EnergyScore + 0.20 × RangeScore
   ```

   `DistanceScore` memberikan prioritas kepada musuh yang lebih dekat, `EnergyScore` memberikan prioritas kepada musuh dengan energi lebih rendah, sedangkan `RangeScore` memberikan nilai tambahan kepada musuh yang berada dalam jarak tembak efektif.

## Requirement

Sebelum menjalankan program, pastikan perangkat telah memiliki beberapa dependensi berikut:

- **Java Runtime Environment / JDK** untuk menjalankan Robocode Tank Royale GUI.
- **.NET SDK 8.0** atau versi yang kompatibel untuk melakukan build program C#.
- **Robocode Tank Royale GUI 0.30.0** sebagai arena pertandingan bot.
- **Visual Studio Code** atau editor lain untuk membuka dan mengedit source code.

Untuk mengecek versi Java:

```bash
java -version
```

Untuk mengecek versi .NET:

```bash
dotnet --version
```

## Cara Menjalankan Program

### 1. Clone repository

Clone repository ini ke komputer lokal:

```bash
git clone https://github.com/natandev/Tubes_Terserah.git
```

Masuk ke folder repository:

```bash
cd Tubes_Terserah
```

### 2. Build program bot

Build bot utama:

```bash
dotnet build ./src/main-bot/GreedyMainBot/GreedyMainBot.csproj
```

Build bot alternatif:

```bash
dotnet build ./src/alternative-bots/GreedyClosestBot/GreedyClosestBot.csproj
dotnet build ./src/alternative-bots/GreedyEnergyBot/GreedyEnergyBot.csproj
dotnet build ./src/alternative-bots/GreedySurvivalBot/GreedySurvivalBot.csproj
```

Jika proses build berhasil, terminal akan menampilkan pesan:

```text
Build succeeded.
0 Error(s)
```

### 3. Menjalankan Robocode Tank Royale GUI

Jika file `robocode-tankroyale-gui-0.30.0.jar` tersedia di folder project, jalankan perintah berikut:

```bash
java -jar robocode-tankroyale-gui-0.30.0.jar
```

Setelah Robocode Tank Royale GUI terbuka:

1. Pilih menu **Battle**.
2. Pilih **Start Battle**.
3. Boot bot yang ingin digunakan.
4. Tambahkan bot ke daftar peserta battle.
5. Klik **Start Battle** untuk memulai pertandingan.

## Command Prompt

Jika menggunakan Windows, bot juga dapat dijalankan melalui file `.cmd` yang tersedia pada masing-masing folder bot.

Contoh menjalankan bot utama:

```cmd
cd src\main-bot\GreedyMainBot
GreedyMainBot.cmd
```

Contoh menjalankan bot alternatif:

```cmd
cd src\alternative-bots\GreedyClosestBot
GreedyClosestBot.cmd
```

```cmd
cd src\alternative-bots\GreedyEnergyBot
GreedyEnergyBot.cmd
```

```cmd
cd src\alternative-bots\GreedySurvivalBot
GreedySurvivalBot.cmd
```

## Bash

Jika menggunakan Linux atau macOS, bot dapat dijalankan melalui file `.sh` yang tersedia pada masing-masing folder bot.

Contoh menjalankan bot utama:

```bash
cd src/main-bot/GreedyMainBot
./GreedyMainBot.sh
```

Contoh menjalankan bot alternatif:

```bash
cd src/alternative-bots/GreedyClosestBot
./GreedyClosestBot.sh
```

```bash
cd src/alternative-bots/GreedyEnergyBot
./GreedyEnergyBot.sh
```

```bash
cd src/alternative-bots/GreedySurvivalBot
./GreedySurvivalBot.sh
```

## Kendala Saat Development

Beberapa kendala yang ditemukan selama development adalah sebagai berikut:

1. **File executable terkunci saat build**  
   Masalah ini terjadi ketika bot masih berjalan di Robocode, sehingga file `.exe` tidak dapat ditimpa saat proses build. Solusinya adalah menghentikan Robocode dan proses bot terlebih dahulu.

   ```cmd
   taskkill /F /IM GreedyMainBot.exe
   taskkill /F /IM GreedyClosestBot.exe
   taskkill /F /IM GreedyEnergyBot.exe
   taskkill /F /IM GreedySurvivalBot.exe
   taskkill /F /IM java.exe
   ```

2. **Bot tidak muncul pada Joined Bots**  
   Masalah ini biasanya terjadi karena bot belum berhasil di-build atau terdapat ketidaksesuaian antara nama class, file `.json`, dan file `.csproj`.

3. **Kesalahan sintaks C#**  
   Beberapa error muncul karena perbedaan huruf besar-kecil pada nama variabel, kurang tanda titik koma, atau nama constructor yang tidak sama dengan nama class.

## Struktur Repository

```text
Tubes_Terserah/
├── doc/
│   └── laporan.pdf
├── src/
│   ├── main-bot/
│   │   └── GreedyMainBot/
│   └── alternative-bots/
│       ├── GreedyClosestBot/
│       ├── GreedyEnergyBot/
│       └── GreedySurvivalBot/
└── README.md
```

## Author

| Nama |
|---|
| Natan |
| Rendi |
| Arbani |

Kelompok: **Terserah**  
Mata Kuliah: **Strategi Algoritma**  
Topik: **Implementasi Algoritma Greedy pada Robocode Tank Royale**
