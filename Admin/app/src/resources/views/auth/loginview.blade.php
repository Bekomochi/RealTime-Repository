<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">

    <title>Login</title>
</head>

<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet"
      integrity="sha384-9ndCyUaIbzAi2FUVXJi0CjmCapSmO7SnpJef0486qhLnuZ2cdeRhO02iuK6FUUVM" crossorigin="anonymous">
<body>

<div class="d-flex justify-content-center">
    <form method="post" action="{{url('auth/dologin')}}">
        @csrf <!--csrf対策-->
        <h1 class="text-center">ログイン</h1>

        <p>名前を入力してください<input type="text" name="name"></p>
        <p>パスワードを入力してください<input type="password" name="password"></p>

        <div class="d-flex justify-content-center">
            <input class="btn btn-secondary" type="submit" value="送信">
        </div>
    </form>
</div>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"
        integrity="sha384-geWF76RCwLtnZ8qwWowPQNguL3RmwHVBC9FhGdlKrxdiJJigb/j/68SIy3Te4Bkz"
        crossorigin="anonymous"></script>
</body>
</html>
