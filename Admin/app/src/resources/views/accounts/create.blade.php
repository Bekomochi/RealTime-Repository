<html>
<body>

<h1>新規アカウントを登録</h1>

<form method="post" action="{{ route('accounts.store') }}">
    @csrf
    <p><input type="text" name="name" placeholder="ここに入力してください"></p>
    <p><input type="password" name="password" placeholder="ここに入力してください"></p>
    <input type="submit" value="送信">
</form>

</body>
</html>
