|CI Tool                    |Build Status|
|---------------------------|---|
| App Center, iOS           |  [![Build status](https://build.appcenter.ms/v0.1/apps/07eb9bfd-c818-4869-8870-90a6db8672d4/branches/master/badge)](https://appcenter.ms) |
| App Center, Android       | [![Build status](https://build.appcenter.ms/v0.1/apps/95153c3a-8cd3-4d08-9b61-1bc9ad6a1bb4/branches/master/badge)](https://appcenter.ms)  |

# FaceOff
FaceOff pits two players against eachother to see who can reflect an emotion best! 

The app provides a random emotion that both players will portray. It then uses the facial recognition technology to determine who exhibited the emotion best!

## About
This is an Android, iOS and UWP app created using [Xamarin.Forms](https://www.xamarin.com/forms). The facial recognition uses [Microsoft's Coginitive Services Face API](https://aka.ms/CognitiveServices/FaceAPI). 

## ToDo
To use Microsoft's Coginitive Services Face API from this app, sign up for a [free API Key](https://azure.microsoft.com/pricing/details/cognitive-services/face-api/?WT.mc_id=none-github-bramin) and insert it to the code [here](./Source/FaceOff/Constants/CognitiveServicesConstants.cs#L7). After adding your API Key, remove the diagnostic directive [located here](./Source/FaceOff/Constants/CognitiveServicesConstants.cs#L5).

### Author
Brandon Minnick

Xamarin Customer Success Engineer


![FaceOff Demo](https://github.com/brminnick/Videos/blob/master/FaceOff/FaceOff_GifDemo.gif)
