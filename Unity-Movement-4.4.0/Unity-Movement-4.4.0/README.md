# Unity-Movement
Unity-Movement is a package that uses OpenXR’s tracking layer APIs to expose Meta Quest Pro’s Body Tracking (BT), Eye Tracking (ET), and Face Tracking (FT) capabilities. With this package, developers can leverage tracking to populate VR environments with custom avatars that bring the expressiveness of users into the virtual environments that they create.

### License
The Unity-Movement package is released under the [Oculus License](https://github.com/oculus-samples/Unity-Movement/blob/main/LICENSE). The MIT License applies to only certain, clearly marked documents. If an individual file does not indicate which license it is subject to, then the Oculus License applies.

### Requirements
- Unity 2021.3.26f1 (2021 LTS) or newer installed. For Unity 2022, use 2022.3.11f1 (2022 LTS) or newer. For Unity 2023, use 2023.1.17f1 or newer (Unity 2023.1.x). Unity 2023.2+ however will not currently work due to the fact TMPro has been deprecated.
- v60.0 or newer of the Meta XR SDK. You will need the Meta XR Core SDK and the Meta XR Interaction SDK OVR Integration packages found [on this page](https://assetstore.unity.com/publishers/25353).
- A project set up with these [configuration settings](https://developer.oculus.com/documentation/unity/unity-conf-settings/)

## Getting Started
First, ensure that all of the [requirements](#requirements) are met.

Then, bring this package into the project.
- In Package Manager, click on the add button below the window title and select **Add package from git URL…**, using this URL: https://github.com/oculus-samples/Unity-Movement.git
- To grab a specific version of the package, append the version number with a # to the git URL (i.e. https://github.com/oculus-samples/Unity-Movement.git#1.2.0)
- Alternatively, in package manager, click on the add button below the window title and select **Add package from disk...**, using the package.json located after unzipping one of the releases here: https://github.com/oculus-samples/Unity-Movement/releases

The sample scenes are located under the **Samples/Scenes** folder.

## Unity Setup

If the new scene or an existing scene doesn’t have a GameObject with the OVRCameraRig component, follow the steps:
1. From the Hierarchy tab, look for a Main Camera GameObject.
2. If the Main Camera GameObject is present, right-click Main Camera and click Delete.
3. In the Project tab, expand the Assets > Oculus > VR > Prefab folder and drag and drop the OVRCameraRig prefab into the scene. You can also drag and drop it into the Hierarchy tab.
4. On the Inspector tab, go to OVR Manager > Quest Features.
5. In the General tab, there are options to enable body, face, and eye tracking support. Select Supported or Required for the type of tracking support you wish to add.
6. Under OVRManager's "Permission Requests On Startup" section, enable  Body, Face and Eye Tracking.
7. Ensure that OVRManager's "Tracking Origin Type" is set to "Floor Level".
8. In OVRManager's "Movement Tracking" section select "High" for "Body Tracking Fidelity."
9. In OVRManager's "Movement Tracking" section select "Full Body" for "Body Tracking Joint Set."

Layer index 10, layer index 11, and the HiddenMesh layer must be present in the project for RecalculateNormals to work correctly.

Some Project Settings can be validated via **Movement->Check Project Settings**. For a more thorough check, please use **Oculus->Tools->Project Setup Tool**.

## Rendering Quality
Navigate to your Project Settings (Edit->Project Settings...) and click on
the "Quality" section. If your project uses URP,
then some of these settings might be part the rendering pipeline asset currently
in use. The pipeline picked will be shown in the Quality menu.

The following settings are recommended:
1. Four bones for Skin Weights.
2. 2x or 4x Multi Sampling Anti Aliasing.
3. Full resolution textures.
4. Shadow settings:
    - Hard and soft shadows.
    - Very high shadow resolution.
    - Stable fit.
    - Shadow distance of 3 meters with cascades. This will allow viewing shadows
nearby without experiencing poor quality.
5. At least one pixel light.

## Player Settings

Make sure that the color space is set to Linear.

## Build Settings

In order for the SceneSelectMenu buttons to work, add the scenes located in the **Samples/Scenes** folder of the package.

## Samples
The project contains several sample scenes. To test the samples, add the scenes located in the **Packages/com.meta.movement/Samples/Scenes** folder to the project's Assets folder.

For more information about body tracking, please refer to [this page](https://developer.oculus.com/documentation/unity/move-body-tracking/).

For more information about the samples, read [Aura Sample](https://developer.oculus.com/documentation/unity/move-samples/#face-and-eye-tracking-with-aura), [Blendshape Mapping Example Sample](https://developer.oculus.com/documentation/unity/move-samples/#arkit-mapping-with-blendshape-mapping-example), [Hip Pinning Sample](https://developer.oculus.com/documentation/unity/move-samples/#high-fidelity-with-hip-pinning), [High Fidelity Sample](https://developer.oculus.com/documentation/unity/move-samples/#high-fidelity-sample), and [Retargeting Sample](https://developer.oculus.com/documentation/unity/move-samples/#retargeting-with-blue-robot).

## Documentation
The documentation for this package can be found [here](https://developer.oculus.com/documentation/unity/move-overview/).
The API reference for this package can be found [here](https://oculus-samples.github.io/Unity-Movement/).
