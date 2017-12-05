# Unity Fix  5.5 to 5.4

[![Englsh](https://img.shields.io/badge/Language-English-red.svg)](README.md)
![Korean](https://img.shields.io/badge/Language-Korean-lightgrey.svg)

- Unity Version Downgrade 툴

## 기능
- Unity 5.5 버젼을 5.4로 다운그레이드 하면 생기는 아래의 문제들을 해결 해줍니다.
- 게임오브젝트 이름이 사라지는 현상
- 게임오브젝트 활성화 상태가 off 되는 현상

## 사용법
- 5.5 프로젝트를 열고 Edit/Project Settings/Editor의 Asset Serialization Mode를 Force Text로 설정해 줍니다. (중요)
- 5.5 프로젝트의 Assets폴더를 복사해, "Put your asset folder here"에 복사해서 넣어줍니다.
- UnityDowngrader.exe를 실행 후 Enter를 입력합니다.
- 변환이 될 동안 5.5 프로젝트를 종료후 프로젝트의 Library, Temp, Assets 폴더를 지워 줍니다
- 변환이 완료되면, 변환이 끝난 Assets폴더를 복사해 5.5 프로젝트에 붙여넣어 줍니다.
- Unity 5.4를 실행 후 5.5 프로젝트를 엽니다.
- 버젼이 불일치하다는 메세지가 노출되어도 Continue 버튼을 눌러 계속해서 진행해주세요.

## 알려진 이슈
- Sub 모듈을 사용하는 파티클 시스템은 Sub 모듈을 재설정합니다.
- 프로젝트가 원래 5.5 이전에 생성 된 경우 5.5로 업그레이드 한 후에 프로젝트에 추가 된 Texture/Sprite는 설정을 재설정해야합니다. 텍스처 유형을 다른 것으로 변경 한 다음 다시 원래대로 변경하는 것만 큼 간단합니다. mipmaps \ compression 설정이 예상 한대로 설정되어 있는지 확인하십시오.
- 프로젝트 버전이 5.5 인 동안 추가 된 모든 Image\Button 구성 요소가 손상됩니다. 이러한 구성 요소를 수동으로 제거하고 추가하거나 소스 코드를 업그레이드하여 해당 구성 요소에 누락 된 GUID를 수정해야합니다 


## 저작권
- 출처 : https://forum.unity.com/threads/behold-the-legendary-unity-5-5-to-5-4-downgrader.457905/
- 모든 저작권은 DoctorShinobi에게 있습니다.
