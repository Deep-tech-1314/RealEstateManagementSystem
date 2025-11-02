# Image Setup Instructions

To fix the image display issue, you need to place the following image files in the `wwwroot/images/properties/` directory:

## Required Image Files:

### Houses (h1.jpg - h5.jpg)
- h1.jpg (Luxury 4BHK Villa in DLF Phase 5)
- h2.jpg (Cozy 3BHK House in Whitefield)
- h3.jpg (Modern Row House in Baner)
- h4.jpg (Traditional House in Anna Nagar)
- h5.jpg (Luxury Bungalow in Juhu)

### Apartments (a1.jpg - a5.jpg)
- a1.jpg (Premium 2BHK in Lodha Altamount)
- a2.jpg (Studio Apartment in Cyber City)
- a3.jpg (Penthouse in Koregaon Park)
- a4.jpg (Lake View Apartment in Powai)
- a5.jpg (Modern 2BHK in Electronic City)

### Villas (v1.jpg - v5.jpg)
- v1.jpg (Beach Villa in ECR Chennai)
- v2.jpg (Hill View Villa in Lonavala)
- v3.jpg (Spanish Villa in Vagator)
- v4.jpg (Smart Villa in Kokapet)
- v5.jpg (Lakeside Villa in Udaipur)

### Commercial (c1.jpg - c5.jpg)
- c1.jpg (Prime Office Space in BKC)
- c2.jpg (Retail Space in Connaught Place)
- c3.jpg (Warehouse in Bhiwandi)
- c4.jpg (IT Office Space in HITEC City)
- c5.jpg (Restaurant Space in Park Street)

### Land (l1.avif - l5.jpg)
- l1.avif (Residential Plot in Sector 150 Noida)
- l2.jpg (Agricultural Land in Nashik)
- l3.jpg (Beach Front Land in Alibaug)
- l4.jpg (Commercial Land on NH-48)
- l5.jpg (Hill Station Plot in Mussoorie)

## Directory Structure:
```
wwwroot/
└── images/
    └── properties/
        ├── h1.jpg, h2.jpg, h3.jpg, h4.jpg, h5.jpg
        ├── a1.jpg, a2.jpg, a3.jpg, a4.jpg, a5.jpg
        ├── v1.jpg, v2.jpg, v3.jpg, v4.jpg, v5.jpg
        ├── c1.jpg, c2.jpg, c3.jpg, c4.jpg, c5.jpg
        └── l1.avif, l2.jpg, l3.jpg, l4.jpg, l5.jpg
```

## Notes:
- You can use any property images you have available
- The images should be in JPG format (except l1.avif)
- Recommended size: 800x600 pixels or similar aspect ratio
- After placing the images, run the application and the seeder will use these local images instead of external URLs
