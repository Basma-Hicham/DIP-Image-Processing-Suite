# DIP Image Processing Suite 📸

A high-performance C# Windows Forms application focused on Digital Image Processing (DIP) techniques. This project demonstrates low-level pixel manipulation and statistical analysis using **OpenCvSharp4**.

## 🚀 Key Features
- **Pointer Math Optimization:** Utilizes `unsafe` C# blocks and pointer arithmetic for direct memory access, ensuring high-speed RGB channel filtering.
- **Histogram Visualization:** Manual calculation of pixel frequencies ($n_i$) for Red, Green, and Blue channels, represented via dynamic charts.
- **Histogram Equalization:** A manual implementation of the equalization algorithm, including Probability ($P_i$) and Cumulative Distribution Function (CDF) calculations.
- **Image Filters:** - Negative Filter
    - Brightness & Contrast Adjustment
    - Grayscale Conversion (Average & Weighted methods)
- **MDI Architecture:** Organized Parent-Child window system (formMain) for a professional user experience.

## 🛠️ Installation & Setup
To run this project locally, follow these steps:

1. **Clone the Repository:**
   ```bash
   git clone [https://github.com/Basma-Hicham/DIP-Image-Processing-Suite.git](https://github.com/Basma-Hicham/DIP-Image-Processing-Suite.git)
