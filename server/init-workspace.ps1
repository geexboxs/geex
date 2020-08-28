function link ($source,$target) {
    cmd /c mklink /D $target $source
}
link ..\..\shared\contracts\ .\shared\contracts\
