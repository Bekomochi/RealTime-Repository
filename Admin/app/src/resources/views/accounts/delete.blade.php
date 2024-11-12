<html>
<body>

<h1>アカウントを削除しますか？</h1>

<form method="post" action="{{route('accounts.destroy')}}">
    @csrf
    <input type="hidden" name="id" value="{{$id}}">
    <input type="submit" value="はい">
</form>
<form action="{{route('accounts.index')}}">
    @csrf
    <input type="submit" value="戻る">
</form>

</body>
</html>
