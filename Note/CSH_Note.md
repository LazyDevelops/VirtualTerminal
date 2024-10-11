rwxr-x code bin

drwxr-x code bin  
-rw-r-- code hello.txt

￦- : 일반 파일  
d : 디렉터일 파일(폴더)

2자리 8진수  
앞자리는 소유자  
뒷자리는 기타

rwxr-x  
111101

r = 4, w = 2, x = 1

ls -1  
자세히 표시  
ls -a  
숨김 파일 표시

```cs
private struct FileSystemEntry{
    public string name { get; }
    public byte permission { get; }
    public string UID { get; }
    public enum FileType { 
        F, D 
    } public FileType fileType { get; }
    public string? content { get; }

    public FileSystemEntry(string name, string UID, byte permission, FileType fileType, string? content = null){
        this.name = name;
        this.UID = UID;
        this.permission = permission;
        this.fileType = fileType;
        this.content = content;
    }
}
```

```cs
Tree<FileSystemNode> root = new Tree<FileSystemNode>(new FileSystemEntry("/", "root", 0b111101, 1));

CreateFile(root, "/", new FileSystemEntry("root", "root", 0b111000, 1));
CreateFile(root, "/", new FileSystemEntry("home", "root", 0b111101, 1));
Tree<FileSystemNode> HOME = CreateFile(root, "/home", new FileSystemEntry($"{USER}", USER, 0b111101, 1));
CreateFile(root, $"/home/{USER}", new FileSystemEntry($"Hello_{USER}.txt", "root", 0b111111, 0, $"Hello, {USER}!"));
```

```cs
bool RemoveFile("상대 주소 or 절대 주소")

Tree<FileSystemNode> FindFile("상대 주소 or 절대 주소")
```

상대 주소 예시  
./ : 현제 디렉터리  
예)  
"pwd는 /home/user라고 가정"  
./Hello.sh = Hello.sh = /home/user/Hello.sh

../ : 이전 디렉터리  
예)  
"pwd는 /home/user라고 가정"  
../ = /home  
../../ = /

~/ = home 디렉터리  
예)  
"pwd는 /home라고 가정"  
./user = /home/user = ~

## TODO

- 경로 스택으로 관리하게 전부 변경하기

cp 명령어 만들고  
logout 명령어도 목록에 추가하기  
만든 명령어들 목록에 추가하기

숨김 파일 기능 고려

```cs
        foreach (string arg in args)
            {
                // -- 옵션을 위한 코드
                /*if(arg.Contains("--")) {
                    options[arg.Replace("--", "")] = true;
                }else */
                if (arg.Contains('-'))
                {
                    foreach (char c in arg)
                    {
                        if (c != '-')
                        {
                            options[c.ToString()] = true;
                        }
                    }
                }
            }
```

권한 문제 다시 생각해보기  
부모 폴더 중 그 어떠한 것이라도  
권한이 없다면 접근 불가

err메세지 따로 분리

# 추가 명령어

    - whoami
    - cp
    - mv
    - rm
    - chmod

# 추가 고려중

    - wc

입출력 제지정자 추가 고려 <, >

파이프는X  
만들기 너무 힘듬

<< 구현 X

지금 cat에 파일 없으면  
자동으로 파일 생성하는  
기능 대신

cat > F

고려

파일 편집기  
gedit 고려

# 예상 명령어 사용 방법 작성 양식

    - cat [옵션] 파일명*  
    - cd [옵션] 경로*  
    - chmod [옵션] 권한* 파일명*  
    - clear  
    - cp [옵션] 파일명* 파일명*  
    - date  
    - echo 문자열*  
    - exit  
    - help  
    - ls [옵션] 경로  
    - man 명령어*  
    - mkdir [옵션] 디렉터리명*  
    - mv [옵션] 파일명* 파일명*  
    - pwd  
    - rm [옵션] 파일명*  
    - rmdir [옵션] 디렉터리명*  
    - whoami

*은 필수 인자
[]는 옵션

# 기본 명력어 목록

    - chmod
    - clear
    - cp*
    - date
    - echo
    - exit
    - help
    - ls
    - man
    - mkdir
    - mv*
    - pwd
    - rm
    - rmdir
    - whoami

# 게임 조작 명령어

    - go
    - get
    - unlock
    - mappin

# 구현중 or 구현 완료된 명령어

    - chmod
    - clear
    - date
    - echo
    - exit
    - help
    - ls
    - man
    - mkdir
    - pwd
    - rmdir
    - whoami

touch 명령어 제작 고려

입력 제지정자를 위해 command는  
string값을 반환하고 그걸 VirtualTerminal에서
출력하게 코드 변경

옵션 검사 코드 수정 필요  
(없는 옵션까지 받고 있음)

Ctrl + D = EOF
Ctrl + C = 강제 종료    

위쪽 화살표 = 전에 입력한 명령어  
아레 화살표 = 후에 입력한 명령어

mkdir -p 구현  
(하위 디렉터리까지 생성)

cat -n 구현  
(라인 번호 표시)  
-b 구현은 고려  
(비어있는 라인은 표시 X)

touch 구현 고려

sleep 구현 고려  
(지정한 시간만큼 대기)

cal 구현 고려  
(달력 출력)

uptime 구현 고려  
(시스템 부팅 시간 출력)

wc 구현 고려  
(파일의 라인, 단어, 문자 수 출력)

; 구현 고려  
(명령어 연달아 실행)
(예: ls; pwd)  
(예: date; sleep 3m; date)

명령어 자동 완성 구현 고려  
(예: ls -> ls)  
(예: ls /h -> ls /home)

grep 명령어 구현고려  
(예: grep "Hello" hello.txt)  

help; help;  
위에 있는 명령어 치면 에러 뜸  
수정 예정