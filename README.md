[![Xamarins](https://github.com/brminnick/FaceOff/actions/workflows/mobile.yml/badge.svg)](https://github.com/brminnick/FaceOff/actions/workflows/mobile.yml)

# FaceOff
FaceOff pits two players against eachother to see who can reflect an emotion best! 

The app provides a random emotion that both players will portray. It then uses the facial recognition technology to determine who exhibited the emotion best!

## About
This is an Android, iOS and UWP app created using [Xamarin.Forms](https://docs.microsoft.com/xamarin/xamarin-forms?WT.mc_id=mobile-0000-bramin). The facial recognition uses [Microsoft's Coginitive Services Face API](https://azure.microsoft.com/services/cognitive-services/face?WT.mc_id=mobile-0000-bramin). 

## ToDo
To use Microsoft's Coginitive Services Face API from this app, sign up for a [free API Key](https://azure.microsoft.com/free/ai/?utm_source=channel9&utm_medium=descriptionlinks&utm_campaign=freeaccount&WT.mc_id=mobile-0000-bramin) and insert it to the code [here](./Source/FaceOff/Constants/CognitiveServicesConstants.cs#L7). After adding your API Key, remove the diagnostic directive [located here](./Source/FaceOff/Constants/CognitiveServicesConstants.cs#L5).

### Author
Brandon Minnick

Xamarin Customer Success Engineer


![FaceOff Demo](https://github.com/brminnick/Videos/blob/master/FaceOff/FaceOff_GifDemo.gif)
