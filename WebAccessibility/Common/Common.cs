using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Net;

namespace WebAccessibility
{
    public class Common
    {
        public struct _General
        {
            public string PrjectTitle;              // 프로젝트 제목
            public int MaxPageCount;           // 최대 허용 페이지 수

            public int MaxTcpIpCount;          // 최대소켓스레드 수 
            public int MaxTcpIpCountPerIP;      // ip당 최대소켓 스레드 수

            public int MaxDepth;                // 최대 디렉토리 단계
            public int MaxPageSize; // kb      // 페이지당 최대 크기제한
            public int MaxParamCountPerPage; // 페이지당 파라미터 수

            public int MaxParamCountPerBoard; // 게시판당 파라미터 수

            public string FilterDomain;             // 내/외부 도메인 리스트

        };

        public struct _Group
        {
            public bool AllowCustomTag;     // 사용자태그 허용여부
            public bool AllowXmlTag;        // xml방식의 태그 허용
            public bool CheckRobot;         // robot.txt 처리
            public bool ScanOtherSite;      // 외부 노드 검색 여부
            public bool Exclude4xx;         // 4xx에러 무시
            public bool Exclude5xx;         // 5xx에러 무시
            public bool ScanFormAction;     // form action 진단
            public bool ScanFromActionInfo; // form action 정보 진단
            public bool SortingUrlQuery;    // url의 인자 정렬
            public bool MoveFirstNoValueInUrlQuery; // 값이 없는 인자를 맨앞으로
            public bool CheckDirPermission; // 디렉토리 퍼미션 진단
            public bool CheckRemoveUrlQuery;   //url 인자 제외한 페이지 진단
            public bool ScanScript;            // <script></script>처리
            public bool ScanScriptEvent;       // {vb|java}script:event 처리
            public bool ScanFlashSrc;           // 플래쉬파일에 포함된 url 검색

            public bool CehckDupleOutbound;     // 중복된 아웃바운드 무시
            public bool CheckUpDir;             // ../ 링크 구문오류를 무시
            public bool CheckDupleSlash;        // // 링크 구문오류를 무시
            public bool CheckQuestionLink;      // ?가 생략된 구문오류를 무시
        };

        public struct _Optional
        {
            public bool CheckUsedTags;          // 각 페이별 태그 사용 여부를 검사함
            public bool CheckMultiMediaSrc;     // 멀티미더 관련파일들의src를 검사함
            public bool CheckAltLength;         // alt문자열의 길이를 채크
            public bool CheckAltValid;          // 비정상적인 alt 문자열 체크
            public bool CheckHeightWidth;       // height/width 속성이 누락된 페이지들을 검사함
            public bool CheckFrameTitle;        // 프레임 타이틀 누락 검사

            public bool CheckServerImageMap;    // 서버사이드 이미지맵을 검사

            public bool CheckClientImageMap;    // 클라이언트 이미지맵을 검사

            public bool CheckOnlyMouseEvent;    // 마우스이벤트로만 사용가능 속성 체킹
            public bool CheckChangeColor;       // 명시적으로 칼라변경된 태그 검사

            public bool CheckStandardHtml;      // 표준 태그집합에 속함 검사

            public bool CheckXmlNameSpaceHtml;     // xml네임스페이스를 사용한 태그를 표준 태그 검사시에 무시
            public bool CheckFrameset;          // frameset에 관련된 정보를 검사함
            public bool CheckJavaApplet;        // java 애플릿 검사

            public bool CheckActiveX;           // activeX 체크
            public bool CheckSubFolder;         // 깊은 페이지/파일 검사

            public bool CheckEtcPort;           // (mms, gopher..) 프로토콜 검사

            public bool CheckDefinedJSFunc;     // javascript 함수가 선언된 곳을 검사

            public bool CheckUsedJSFunc;        // javascript 함수를 호출한 곳을 검사

        };

        public struct _Identify
        {
            public Hashtable BadWordList;           // 유해어

            public Hashtable TabooWordList;         // 금칙어

            public Hashtable SiteIdentifWordList;   // 사이트 고유 식별
            public Hashtable ExcludeWordList;       // 검색예외 단어
            public bool CheckAttachFile;            // 첨부문서를 검사함
            public bool CheckMailTo;                // mailto: 를 검사함
            public bool CheckExlcudeMailTo;         // webmaster@.. 무시
            public bool CheckTextHtml2Depth;        // text/html 2단계 검사

        };

        public struct _UserHTTP
        {
            public Hashtable Out4xxList;
            public Hashtable Out5xxList;
            public Hashtable OutDirList;
        };

        public struct _Etc
        {
            public ArrayList HttpUserAgentList;
            public string ReportOutDir;
            public bool CheckFileExtension;
            public bool CheckIgonoreCase;
            public string FilterFileExtension;
            public int MaxCookieCount;
            public int MaxCookieSize;
        };

        public struct _Accessibility
        {
            public int MaxAltLength;
            public int MaxAltWordCount;
            public ArrayList InValidAltWord;
            public ArrayList SaveTagList;
        };


        public struct ProjectConfig
        {
            public _General General;
            public _Accessibility Accessibility;
            public _Etc Etc;
            public _Group Group;
            public _Identify Identify;
            public _Optional Optional;
            public _UserHTTP UserHTTP;
        };

    }
}
