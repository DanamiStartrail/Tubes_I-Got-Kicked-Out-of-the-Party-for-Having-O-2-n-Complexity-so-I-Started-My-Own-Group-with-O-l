# Tugas Besar Strategi Algoritma - Robocode Tank Royale

Repositori ini berisi implementasi bot simulasi pertempuran untuk Robocode Tank Royale yang dikembangkan menggunakan bahasa pemrograman C# (.NET). Bot ini mengimplementasikan algoritma *Greedy* untuk mengambil keputusan lokal terbaik pada setiap *tick* simulasi.

## i. Penjelasan Singkat Algoritma Greedy

Bot utama (`MainBot`) mengimplementasikan strategi **Adaptive Predictive Greedy** yang membagi fokus heuristik ke dalam tiga subsistem independen (asinkron):
1.  **Greedy Penargetan (Predictive Aiming):** Bot tidak menembak posisi target saat ini, melainkan memprediksi posisi masa depan target menggunakan *Linear Predictive Targeting* yang diperhalus dengan *Moving Average Filter* (3-tick). Hal ini memaksimalkan probabilitas peluru mengenai sasaran (akurat).
2.  **Greedy Daya Tembak (Distance-Based Firepower):** Bot secara adaptif memilih daya tembak tertinggi ($3.0$) saat musuh berada di radius dekat ($< 200$ piksel) untuk penetrasi maksimal, dan menurunkan daya ($1.5$) pada radius jauh untuk menghemat energi dan meningkatkan kecepatan luncur proyektil.
3.  **Greedy Pergerakan (Circle Strafing & Wall Avoidance):** Bot selalu bergerak menyamping tegak lurus ($90^\circ$) dari arah musuh untuk mempersulit bidikan lawan. Jika bot mendeteksi keberadaan dinding pada batas $80$ piksel, heuristik proaktif akan membatalkan *strafing* sesaat dan memaksa bot berbelok menuju pusat arena untuk menghindari benturan yang merugikan.

## ii. Requirement Program

Untuk menjalankan dan mengkompilasi bot ini, sistem Anda memerlukan perangkat lunak berikut:
1.  **.NET SDK (versi 6.0 atau lebih baru):** Diperlukan untuk melakukan *build* dan menjalankan kode C#.
2.  **Robocode Tank Royale Server:** Server lokal untuk menjalankan simulasi arena.
3.  **Robocode Tank Royale UI:** Antarmuka grafis (GUI) untuk memvisualisasikan pertempuran dan mengatur parameter *match*.
4.  **Robocode Tank Royale Bot API for .NET:** *Library* bawaan yang telah direferensikan dalam *file* `.csproj`.

## iii. Langkah-Langkah Compile & Build Program

Berikut adalah langkah-langkah untuk menjalankan bot di lingkungan lokal Anda:

1.  **Jalankan Server Robocode:**
    Buka terminal/CMD dan jalankan file *booter* server Robocode Tank Royale (`robocode-tankroyale-server.bat` atau `.sh`). Pastikan server berjalan di *port* default.
2.  **Jalankan UI Robocode:**
    Buka antarmuka pengguna (`robocode-tankroyale-ui.bat` atau `.sh`) untuk melihat arena.
3.  **Konfigurasi Environment Variable (Opsional namun disarankan):**
    Pastikan bot dapat melakukan *handshake* dengan server. Jika server menggunakan kunci rahasia, atur variabel *environment*:
    * *Windows:* `set ROBOCODE_SECRET=rahasia`
    * *Linux/Mac:* `export ROBOCODE_SECRET=rahasia`
    
    * Atau gunakan perintah ini untuk memasukkan kunci rahasia:
      ```bash
      export SERVER_SECRET="qJXYf4xcmXTPh6ywAV/plzfGrb7sDM38dCMDuu2hkH"
      ```
4.  **Kompilasi dan Jalankan Bot:**
    Buka terminal baru, arahkan direktori kerja ke dalam *folder* proyek yang berisi file `.csproj`, lalu jalankan perintah berikut:
    ```bash
    dotnet build
    dotnet run
    ```
    *(Tunggu hingga muncul pesan indikasi bahwa bot berhasil terhubung ke server).*
5.  **Mulai Pertandingan:**
    Kembali ke jendela UI Robocode, pilih bot yang baru saja diaktifkan dari daftar partisipan, dan klik "Start Battle".

## iv. Author (Identitas Pembuat)

**Kelompok:** KickedOutOfTheParty
* *Mandanta Guru Singa* - *(124140147)*
* *Rachman Hady* - *(124140129)*
* *Gathan Mahendra* - *(124140201)*
