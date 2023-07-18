export class CommenttModelRequest{
  topicURL!: string;
  parrentId!: string;
  userId!: string;
  commentText!: string;
}

export class CommentModelResponse{
  TopicURL!: string;
  commentId!: string;
  parrentId!: string;
  userId!: string;
  userNickName!: string;
  commentText!: string;
  createdAt!: Date;
  updatedAt!: Date;
  likes!: Like[];
  disLikes!: DisLike[];
  replies!: CommentModelResponse[];
}

export class Action{
  commentId!: string;
  userId!: string;
}
export class Like extends Action{}

export class DisLike extends Action{}
